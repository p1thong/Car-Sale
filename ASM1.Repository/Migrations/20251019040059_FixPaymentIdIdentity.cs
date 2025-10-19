using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM1.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentIdIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix Payment table to make paymentId an IDENTITY column
            migrationBuilder.Sql(@"
-- Create temporary table with IDENTITY paymentId
CREATE TABLE Payment_temp (
    paymentId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    orderId int NOT NULL,
    amount decimal(12,2) NULL,
    method varchar(50) NULL,
    PaymentMethod nvarchar(max) NULL,
    paymentDate datetime2 NULL,
    Status nvarchar(max) NULL
);

-- Copy existing data (excluding paymentId to let IDENTITY generate new values)
IF EXISTS(SELECT 1 FROM Payment)
BEGIN
    SET IDENTITY_INSERT Payment_temp OFF;
    INSERT INTO Payment_temp (orderId, amount, method, PaymentMethod, paymentDate, Status)
    SELECT orderId, amount, method, PaymentMethod, paymentDate, Status FROM Payment;
END

-- Drop constraints and indexes on original table
IF EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK__Payment__orderId__7D439ABD')
    ALTER TABLE Payment DROP CONSTRAINT FK__Payment__orderId__7D439ABD;

IF EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'IX_Payment_orderId')
    DROP INDEX IX_Payment_orderId ON Payment;

-- Replace original table
DROP TABLE Payment;
EXEC sp_rename 'Payment_temp', 'Payment';

-- Recreate foreign key and index
ALTER TABLE Payment WITH CHECK ADD CONSTRAINT FK__Payment__orderId__7D439ABD 
    FOREIGN KEY(orderId) REFERENCES [Order](orderId);
CREATE INDEX IX_Payment_orderId ON Payment(orderId);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert paymentId back to non-IDENTITY (this will lose auto-generation capability)
            migrationBuilder.Sql(@"
-- Create table without IDENTITY
CREATE TABLE Payment_old (
    paymentId int NOT NULL,
    orderId int NOT NULL,
    amount decimal(12,2) NULL,
    method varchar(50) NULL,
    PaymentMethod nvarchar(max) NULL,
    paymentDate datetime2 NULL,
    Status nvarchar(max) NULL
);

-- Copy data back
INSERT INTO Payment_old (paymentId, orderId, amount, method, PaymentMethod, paymentDate, Status)
SELECT paymentId, orderId, amount, method, PaymentMethod, paymentDate, Status FROM Payment;

-- Drop constraints and replace table
IF EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK__Payment__orderId__7D439ABD')
    ALTER TABLE Payment DROP CONSTRAINT FK__Payment__orderId__7D439ABD;
DROP INDEX IX_Payment_orderId ON Payment;

DROP TABLE Payment;
EXEC sp_rename 'Payment_old', 'Payment';

-- Recreate constraints
ALTER TABLE Payment ADD CONSTRAINT PK__Payment__A0D9EFC6E641B2C2 PRIMARY KEY (paymentId);
ALTER TABLE Payment WITH CHECK ADD CONSTRAINT FK__Payment__orderId__7D439ABD 
    FOREIGN KEY(orderId) REFERENCES [Order](orderId);
CREATE INDEX IX_Payment_orderId ON Payment(orderId);
");
        }
    }
}
