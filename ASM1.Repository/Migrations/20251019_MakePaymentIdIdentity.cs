using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM1.Repository.Migrations
{
    public partial class MakePaymentIdIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to replace Payment table with identity PK
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payment]') AND type = 'U')
BEGIN
    -- Create temporary table with identity
    IF OBJECT_ID('dbo.Payment_temp', 'U') IS NOT NULL
        DROP TABLE dbo.Payment_temp;

    CREATE TABLE dbo.Payment_temp (
        paymentId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        orderId int NOT NULL,
        amount decimal(12,2) NULL,
        method varchar(50) NULL,
        PaymentMethod nvarchar(max) NULL,
        paymentDate datetime2 NULL,
        Status nvarchar(max) NULL
    );

    -- Copy data (leave paymentId generation to identity)
    INSERT INTO dbo.Payment_temp (orderId, amount, method, PaymentMethod, paymentDate, Status)
    SELECT orderId, amount, method, PaymentMethod, paymentDate, Status FROM dbo.Payment;

    -- Drop old table and rename
    DROP TABLE dbo.Payment;
    EXEC sp_rename 'dbo.Payment_temp', 'Payment';

    -- Recreate index and foreign key
    CREATE INDEX IX_Payment_orderId ON dbo.Payment(orderId);
    ALTER TABLE dbo.Payment WITH CHECK ADD CONSTRAINT FK__Payment__orderId__7D439ABD FOREIGN KEY(orderId) REFERENCES dbo.[Order](orderId);
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Attempt to revert: recreate non-identity Payment if needed (may lose identity values)
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Payment]') AND type = 'U')
BEGIN
    IF OBJECT_ID('dbo.Payment_old', 'U') IS NOT NULL
        DROP TABLE dbo.Payment_old;

    CREATE TABLE dbo.Payment_old (
        paymentId int NOT NULL,
        orderId int NOT NULL,
        amount decimal(12,2) NULL,
        method varchar(50) NULL,
        PaymentMethod nvarchar(max) NULL,
        paymentDate datetime2 NULL,
        Status nvarchar(max) NULL
    );

    INSERT INTO dbo.Payment_old (paymentId, orderId, amount, method, PaymentMethod, paymentDate, Status)
    SELECT paymentId, orderId, amount, method, PaymentMethod, paymentDate, Status FROM dbo.Payment;

    DROP TABLE dbo.Payment;
    EXEC sp_rename 'dbo.Payment_old', 'Payment';

    CREATE INDEX IX_Payment_orderId ON dbo.Payment(orderId);
    ALTER TABLE dbo.Payment WITH CHECK ADD CONSTRAINT FK__Payment__orderId__7D439ABD FOREIGN KEY(orderId) REFERENCES dbo.[Order](orderId);
END
");
        }
    }
}
