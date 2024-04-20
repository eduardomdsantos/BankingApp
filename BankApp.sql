CREATE TABLE [PersonalInfo] (
  [PersonalInfoID] int IDENTITY(1,1) PRIMARY KEY,
  [FirstName] nvarchar(255),
  [Surname] nvarchar(255),
  [Email] nvarchar(255),
  [Phone] nvarchar(255),
  [AddressLine1] nvarchar(255),
  [AddressLine2] nvarchar(255),
  [City] nvarchar(255),
  [County] nvarchar(255)
)

CREATE TABLE [User] (
  [UserID] int IDENTITY(1,1) PRIMARY KEY,
  [PersonalInfoID] int,
  [Username] nvarchar(255),
  [PasswordHash] nvarchar(255),
  CONSTRAINT FK_User_PersonalInfo FOREIGN KEY ([PersonalInfoID]) REFERENCES [PersonalInfo] ([PersonalInfoID])
)

CREATE TABLE [Account] (
  [AccountID] int IDENTITY(1,1) PRIMARY KEY,
  [PersonalInfoID] int,
  [AccountNumber] int,
  [SortCode] int,
  [AccountType] nvarchar(255),
  [Balance] decimal,
  [OverdraftLimit] decimal,
  CONSTRAINT FK_Account_PersonalInfo FOREIGN KEY ([PersonalInfoID]) REFERENCES [PersonalInfo] ([PersonalInfoID])
)

CREATE TABLE [Transaction] (
  [TransactionID] int IDENTITY(1,1) PRIMARY KEY,
  [UserID] int,
  [AccountID] int,
  [TransactionType] nvarchar(255),
  [Amount] decimal,
  [TransactionDate] datetime,
  [TransferReference] nvarchar(255),
  [RecipientBankName] nvarchar(255),
  [RecipientAccountNumber] nvarchar(255),
  [RecipientAccountHolderName] nvarchar(255),
  [IntermediaryBankName] nvarchar(255),
  [IntermediaryBankAccountNumber] nvarchar(255),
  CONSTRAINT FK_Transaction_User FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID]),
  CONSTRAINT FK_Transaction_Account FOREIGN KEY ([AccountID]) REFERENCES [Account] ([AccountID])
)

GO

CREATE PROCEDURE AddUser
    @FirstName NVARCHAR(255),
    @Surname NVARCHAR(255),
    @Email NVARCHAR(255),
    @Phone NVARCHAR(255),
    @AddressLine1 NVARCHAR(255),
    @AddressLine2 NVARCHAR(255),
    @City NVARCHAR(255),
    @County NVARCHAR(255),
    @Username NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    DECLARE @PersonalInfoID INT
    DECLARE @PasswordSalt NVARCHAR(255)
    DECLARE @PasswordHash NVARCHAR(255)

    SET @PasswordSalt = CONVERT(NVARCHAR(36), NEWID())

    SET @PasswordHash = HASHBYTES('SHA2_512', @Password + @PasswordSalt)

    INSERT INTO PersonalInfo (FirstName, Surname, Email, Phone, AddressLine1, AddressLine2, City, County)
    VALUES (@FirstName, @Surname, @Email, @Phone, @AddressLine1, @AddressLine2, @City, @County)

    SET @PersonalInfoID = SCOPE_IDENTITY()

    INSERT INTO [User] (PersonalInfoID, Username, PasswordHash, PasswordSalt)
    VALUES (@PersonalInfoID, @Username, @PasswordHash, @PasswordSalt)
END
GO

CREATE PROCEDURE UpdateUser
    @UserID INT,
    @FirstName NVARCHAR(255),
    @Surname NVARCHAR(255),
    @Email NVARCHAR(255),
    @Phone NVARCHAR(255),
    @AddressLine1 NVARCHAR(255),
    @AddressLine2 NVARCHAR(255),
    @City NVARCHAR(255),
    @County NVARCHAR(255),
    @Username NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    DECLARE @PasswordSalt NVARCHAR(255) = CONVERT(NVARCHAR(36), NEWID())

    DECLARE @PasswordHash NVARCHAR(255) = HASHBYTES('SHA2_512', @Password + @PasswordSalt)

    UPDATE PersonalInfo
    SET FirstName = @FirstName,
        Surname = @Surname,
        Email = @Email,
        Phone = @Phone,
        AddressLine1 = @AddressLine1,
        AddressLine2 = @AddressLine2,
        City = @City,
        County = @County
    WHERE PersonalInfoID = (SELECT PersonalInfoID FROM [User] WHERE UserID = @UserID)

    UPDATE [User]
    SET Username = @Username,
        PasswordHash = @PasswordHash,
        PasswordSalt = @PasswordSalt
    WHERE UserID = @UserID
END
GO

CREATE PROCEDURE LoginUser
    @Username NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    DECLARE @UserID INT
    DECLARE @PasswordHash NVARCHAR(255)
    DECLARE @PasswordSalt NVARCHAR(255)
    DECLARE @HashedPassword NVARCHAR(255)

    SELECT @UserID = UserID, @PasswordHash = PasswordHash, @PasswordSalt = PasswordSalt
    FROM [User]
    WHERE Username = @Username

    SET @HashedPassword = HASHBYTES('SHA2_512', @Password + @PasswordSalt)

    IF @PasswordHash = @HashedPassword
    BEGIN
        SELECT @UserID AS UserID, 1 AS Authenticated
    END
    ELSE
    BEGIN
        SELECT 0 AS UserID, 0 AS Authenticated
    END
END
GO

CREATE PROCEDURE AddAccount
    @FirstName NVARCHAR(255),
    @Surname NVARCHAR(255),
    @Email NVARCHAR(255),
    @Phone NVARCHAR(255),
    @AddressLine1 NVARCHAR(255),
    @AddressLine2 NVARCHAR(255),
    @City NVARCHAR(255),
    @County NVARCHAR(255),
    @AccountNumber INT,
    @SortCode INT,
    @AccountType NVARCHAR(255),
    @Balance DECIMAL,
    @OverdraftLimit DECIMAL
AS
BEGIN
    DECLARE @PersonalInfoID INT

    INSERT INTO PersonalInfo (FirstName, Surname, Email, Phone, AddressLine1, AddressLine2, City, County)
    VALUES (@FirstName, @Surname, @Email, @Phone, @AddressLine1, @AddressLine2, @City, @County)

    SET @PersonalInfoID = SCOPE_IDENTITY()

    INSERT INTO Account (PersonalInfoID, AccountNumber, SortCode, AccountType, Balance, OverdraftLimit)
    VALUES (@PersonalInfoID, @AccountNumber, @SortCode, @AccountType, @Balance, @OverdraftLimit)
END
GO


CREATE PROCEDURE PerformTransaction
    @SourceAccountID INT,
    @DestinationAccountID INT,
    @Amount DECIMAL,
    @TransactionType NVARCHAR(50),
    @UserID INT
AS
BEGIN
    DECLARE @SourceBalance DECIMAL
    DECLARE @DestinationBalance DECIMAL

    BEGIN TRY
        BEGIN TRANSACTION;
        
        SELECT @SourceBalance = Balance FROM Account WHERE AccountID = @SourceAccountID
        SELECT @DestinationBalance = Balance FROM Account WHERE AccountID = @DestinationAccountID

        IF @TransactionType = 'Lodge' OR @TransactionType = 'BankTransfer'
        BEGIN
            UPDATE Account SET Balance = @DestinationBalance + @Amount WHERE AccountID = @DestinationAccountID
            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate)
            VALUES (@UserID, @DestinationAccountID, 'Lodgement', @Amount, GETDATE())
        END
        
        IF @TransactionType = 'Withdraw' OR @TransactionType = 'BankTransfer'
        BEGIN
            UPDATE Account SET Balance = @SourceBalance - @Amount WHERE AccountID = @SourceAccountID
            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate)
            VALUES (@UserID, @SourceAccountID, 'Withdrawal', @Amount, GETDATE())
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO
