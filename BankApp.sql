IF OBJECT_ID('dbo.PerformTransaction', 'P') IS NOT NULL
    DROP PROCEDURE dbo.PerformTransaction;

IF OBJECT_ID('dbo.UpdateAccount', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateAccount;

IF OBJECT_ID('dbo.AddAccount', 'P') IS NOT NULL
    DROP PROCEDURE dbo.AddAccount;

IF OBJECT_ID('dbo.LoginUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.LoginUser;

IF OBJECT_ID('dbo.UpdateUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateUser;

IF OBJECT_ID('dbo.AddUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.AddUser;

IF OBJECT_ID('dbo.[Transaction]', 'U') IS NOT NULL
    DROP TABLE dbo.[Transaction];

IF OBJECT_ID('dbo.Account', 'U') IS NOT NULL
    DROP TABLE dbo.Account;

IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL
    DROP TABLE dbo.[User];

IF OBJECT_ID('dbo.PersonalInfo', 'U') IS NOT NULL
    DROP TABLE dbo.PersonalInfo;


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
  [PasswordSalt] nvarchar(255),
  CONSTRAINT FK_User_PersonalInfo FOREIGN KEY ([PersonalInfoID]) REFERENCES [PersonalInfo] ([PersonalInfoID])
)

GO

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
    [TransactionID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT,
    [AccountID] INT,
    [TransactionType] NVARCHAR(255),
    [Amount] DECIMAL,
    [TransactionDate] DATETIME,
    [ReferenceNumber] NVARCHAR(255),
    [RecipientBankName] NVARCHAR(255),
    [RecipientAccountNumber] NVARCHAR(255),
    [RecipientAccountHolderName] NVARCHAR(255),
    [IntermediaryBankName] NVARCHAR(255),
    [IntermediaryBankAccountNumber] NVARCHAR(255),
    [IsTransfer] CHAR(1), 
    CONSTRAINT FK_Transaction_User FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID]),
    CONSTRAINT FK_Transaction_Account FOREIGN KEY ([AccountID]) REFERENCES [Account] ([AccountID])
);
GO

CREATE OR ALTER PROCEDURE AddUser
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

CREATE OR ALTER PROCEDURE UpdateUser
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

CREATE OR ALTER PROCEDURE LoginUser
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

CREATE OR ALTER PROCEDURE AddAccount
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

CREATE OR ALTER PROCEDURE UpdateAccount
    @AccountID INT,
    @Email NVARCHAR(255),
    @Phone NVARCHAR(255),
    @AddressLine1 NVARCHAR(255),
    @AddressLine2 NVARCHAR(255),
    @City NVARCHAR(255),
    @County NVARCHAR(255),
    @OverdraftLimit DECIMAL
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE PersonalInfo
        SET Email = @Email,
            Phone = @Phone,
            AddressLine1 = @AddressLine1,
            AddressLine2 = @AddressLine2,
            City = @City,
            County = @County
        WHERE PersonalInfoID = (SELECT PersonalInfoID FROM Account WHERE AccountID = @AccountID)

        UPDATE Account
        SET OverdraftLimit = @OverdraftLimit
        WHERE AccountID = @AccountID

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

CREATE OR ALTER PROCEDURE GetUserByID
    @UserID INT
AS
BEGIN
    SELECT 
        U.UserID,
        P.FirstName,
        P.Surname,
        P.Email,
        P.Phone,
        P.AddressLine1,
        P.AddressLine2,
        P.City,
        P.County,
        U.Username
    FROM 
        [User] U
    INNER JOIN 
        PersonalInfo P ON U.PersonalInfoID = P.PersonalInfoID
    WHERE 
        U.UserID = @UserID;
END
GO

CREATE OR ALTER PROCEDURE GetAccountByID
    @AccountID INT
AS
BEGIN
    SELECT 
        A.AccountID,
        P.FirstName,
        P.Surname,
        P.Email,
        P.Phone,
        P.AddressLine1,
        P.AddressLine2,
        P.City,
        P.County,
        A.AccountNumber,
        A.SortCode,
        A.AccountType,
        A.Balance,
        A.OverdraftLimit
    FROM 
        Account A
    INNER JOIN 
        PersonalInfo P ON A.PersonalInfoID = P.PersonalInfoID
    WHERE 
        A.AccountID = @AccountID;
END
GO
CREATE OR ALTER PROCEDURE GetAllAccounts
AS
BEGIN
    SELECT 
        A.AccountID,
        P.FirstName,
        P.Surname,
        P.Email,
        P.Phone,
        P.AddressLine1,
        P.AddressLine2,
        P.City,
        P.County,
        A.AccountNumber,
        A.SortCode,
        A.AccountType,
        A.Balance,
        A.OverdraftLimit
    FROM 
        Account A
    INNER JOIN 
        PersonalInfo P ON A.PersonalInfoID = P.PersonalInfoID;
END
GO

CREATE OR ALTER PROCEDURE GetAllTransactions
AS
BEGIN
    SELECT 
        T.TransactionID,
        U.UserID,
        U.Username,
        A.AccountID,
        A.AccountNumber,
        A.SortCode,
        T.TransactionType,
        T.Amount,
        T.TransactionDate,
        T.ReferenceNumber,
        T.RecipientBankName,
        T.RecipientAccountNumber,
        T.RecipientAccountHolderName,
        T.IntermediaryBankName,
        T.IntermediaryBankAccountNumber,
        T.IsTransfer
    FROM 
        [Transaction] T
    INNER JOIN 
        [User] U ON T.UserID = U.UserID
    INNER JOIN 
        Account A ON T.AccountID = A.AccountID;
END
GO


CREATE OR ALTER PROCEDURE PerformTransaction
    @SourceAccountID INT,
    @DestinationAccountID INT,
    @Amount DECIMAL,
    @TransactionType NVARCHAR(50),
    @UserID INT,
    @ReferenceNumber NVARCHAR(255) = NULL,
    @RecipientBankName NVARCHAR(255) = NULL,
    @RecipientAccountNumber NVARCHAR(255) = NULL,
    @RecipientAccountHolderName NVARCHAR(255) = NULL,
    @IntermediaryBankName NVARCHAR(255) = NULL,
    @IntermediaryBankAccountNumber NVARCHAR(255) = NULL
AS
BEGIN
    DECLARE @SourceBalance DECIMAL
    DECLARE @DestinationBalance DECIMAL

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @SourceBalance = Balance FROM Account WHERE AccountID = @SourceAccountID
        SELECT @DestinationBalance = Balance FROM Account WHERE AccountID = @DestinationAccountID

        IF @TransactionType = 'Lodge'
        BEGIN
            UPDATE Account SET Balance = @SourceBalance + @Amount WHERE AccountID = @SourceAccountID
            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate, IsTransfer)
            VALUES (@UserID, @SourceAccountID, 'Lodge', @Amount, GETDATE(), 'N') 
        END

        IF @TransactionType = 'Withdraw'
        BEGIN
            UPDATE Account SET Balance = @SourceBalance - @Amount WHERE AccountID = @SourceAccountID
            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate, IsTransfer)
            VALUES (@UserID, @SourceAccountID, 'Withdrawal', @Amount, GETDATE(), 'N') 
        END

        IF @TransactionType = 'Internal_Transfer'
        BEGIN
            UPDATE Account SET Balance = @SourceBalance - @Amount WHERE AccountID = @SourceAccountID
            UPDATE Account SET Balance = @DestinationBalance + @Amount WHERE AccountID = @DestinationAccountID

            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate, IsTransfer, ReferenceNumber)
            VALUES (@UserID, @SourceAccountID, 'Withdrawal', @Amount, GETDATE(), 'Y', @ReferenceNumber) 

            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate, IsTransfer, ReferenceNumber)
            VALUES (@UserID, @DestinationAccountID, 'Lodge', @Amount, GETDATE(), 'Y', @ReferenceNumber) 
        END


        IF @TransactionType = 'External_Transfer'
        BEGIN
            UPDATE Account SET Balance = @SourceBalance - @Amount WHERE AccountID = @SourceAccountID
            INSERT INTO [Transaction] (UserID, AccountID, TransactionType, Amount, TransactionDate, RecipientBankName, RecipientAccountNumber, RecipientAccountHolderName, IntermediaryBankName, IntermediaryBankAccountNumber, IsTransfer, ReferenceNumber)
            VALUES (@UserID, @SourceAccountID, 'Withdrawal', @Amount, GETDATE(), @RecipientBankName, @RecipientAccountNumber, @RecipientAccountHolderName, @IntermediaryBankName, @IntermediaryBankAccountNumber, 'Y', @ReferenceNumber) 
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



-- Insert default admin user
EXEC AddUser 
    @FirstName = 'Admin', 
    @Surname = 'Admin', 
    @Email = 'admin@example.com', 
    @Phone = '123456789', 
    @AddressLine1 = '123 Admin Street', 
    @AddressLine2 = '123 Admin Street', 
    @City = 'Admin City', 
    @County = 'Dublin', 
    @Username = 'admin', 
    @Password = 'admin';

GO

DECLARE @i INT = 1
WHILE @i <= 10
BEGIN
    DECLARE @AccountNumber INT = 1000 + @i
    DECLARE @SortCode INT = 100000 + @i

    DECLARE @AccountHolderName NVARCHAR(255) = 'Account Holder Name' + CAST(@i AS NVARCHAR(10))

    EXEC AddAccount 
        @FirstName = @AccountHolderName, 
        @Surname = 'Surname', 
        @Email = 'admin@example.com', 
        @Phone = '123456789', 
        @AddressLine1 = '123 Admin Street', 
        @AddressLine2 = NULL, 
        @City = 'Dublin', 
        @County = 'Dublin', 
        @AccountNumber = @AccountNumber, 
        @SortCode = @SortCode, 
        @AccountType = 'Current', 
        @Balance = 0, 
        @OverdraftLimit = 5000;

    SET @i = @i + 1;
END;