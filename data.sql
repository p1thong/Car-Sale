------------------------------------------------
-- CUSTOMER → DEALER
-------------------------------------------------
ALTER TABLE [dbo].[Customer]
ADD CONSTRAINT FK_Customer_Dealer
FOREIGN KEY ([dealerId]) REFERENCES [dbo].[Dealer]([dealerId])
ON DELETE NO ACTION ON UPDATE CASCADE;
GO

-------------------------------------------------
-- DEALERCONTRACT → DEALER, MANUFACTURER
-------------------------------------------------
ALTER TABLE [dbo].[DealerContract]
ADD CONSTRAINT FK_DealerContract_Dealer
FOREIGN KEY ([dealerId]) REFERENCES [dbo].[Dealer]([dealerId])
ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_DealerContract_Manufacturer
FOREIGN KEY ([manufacturerId]) REFERENCES [dbo].[Manufacturer]([manufacturerId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- FEEDBACK → CUSTOMER
-------------------------------------------------
ALTER TABLE [dbo].[Feedback]
ADD CONSTRAINT FK_Feedback_Customer
FOREIGN KEY ([customerId]) REFERENCES [dbo].[Customer]([customerId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- ORDER → DEALER, CUSTOMER, VEHICLEVARIANT
-------------------------------------------------
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT FK_Order_Dealer
FOREIGN KEY ([dealerId]) REFERENCES [dbo].[Dealer]([dealerId])
ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Order_Customer
FOREIGN KEY ([customerId]) REFERENCES [dbo].[Customer]([customerId])
ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Order_VehicleVariant
FOREIGN KEY ([variantId]) REFERENCES [dbo].[VehicleVariant]([variantId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- PAYMENT → ORDER
-------------------------------------------------
ALTER TABLE [dbo].[Payment]
ADD CONSTRAINT FK_Payment_Order
FOREIGN KEY ([orderId]) REFERENCES [dbo].[Order]([orderId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- PROMOTION → ORDER
-------------------------------------------------
ALTER TABLE [dbo].[Promotion]
ADD CONSTRAINT FK_Promotion_Order
FOREIGN KEY ([orderId]) REFERENCES [dbo].[Order]([orderId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- QUOTATION → CUSTOMER, DEALER, VEHICLEVARIANT
-------------------------------------------------
ALTER TABLE [dbo].[Quotation]
ADD CONSTRAINT FK_Quotation_Customer
FOREIGN KEY ([customerId]) REFERENCES [dbo].[Customer]([customerId])
ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_Quotation_Dealer
FOREIGN KEY ([dealerId]) REFERENCES [dbo].[Dealer]([dealerId])
ON DELETE NO ACTION ON UPDATE CASCADE,
CONSTRAINT FK_Quotation_VehicleVariant
FOREIGN KEY ([variantId]) REFERENCES [dbo].[VehicleVariant]([variantId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- SALESCONTRACT → ORDER
-------------------------------------------------
ALTER TABLE [dbo].[SalesContract]
ADD CONSTRAINT FK_SalesContract_Order
FOREIGN KEY ([orderId]) REFERENCES [dbo].[Order]([orderId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- TESTDRIVE → CUSTOMER, VEHICLEVARIANT
-------------------------------------------------
ALTER TABLE [dbo].[TestDrive]
ADD CONSTRAINT FK_TestDrive_Customer
FOREIGN KEY ([customerId]) REFERENCES [dbo].[Customer]([customerId])
ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT FK_TestDrive_VehicleVariant
FOREIGN KEY ([variantId]) REFERENCES [dbo].[VehicleVariant]([variantId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- USER → DEALER, MANUFACTURER
-------------------------------------------------
ALTER TABLE [dbo].[User]
ADD CONSTRAINT FK_User_Dealer
FOREIGN KEY ([dealerId]) REFERENCES [dbo].[Dealer]([dealerId])
ON DELETE SET NULL ON UPDATE CASCADE,
CONSTRAINT FK_User_Manufacturer
FOREIGN KEY ([manufacturerId]) REFERENCES [dbo].[Manufacturer]([manufacturerId])
ON DELETE SET NULL ON UPDATE CASCADE;
GO

-------------------------------------------------
-- VEHICLEMODEL → MANUFACTURER
-------------------------------------------------
ALTER TABLE [dbo].[VehicleModel]
ADD CONSTRAINT FK_VehicleModel_Manufacturer
FOREIGN KEY ([manufacturerId]) REFERENCES [dbo].[Manufacturer]([manufacturerId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO

-------------------------------------------------
-- VEHICLEVARIANT → VEHICLEMODEL
-------------------------------------------------
ALTER TABLE [dbo].[VehicleVariant]
ADD CONSTRAINT FK_VehicleVariant_VehicleModel
FOREIGN KEY ([vehicleModelId]) REFERENCES [dbo].[VehicleModel]([vehicleModelId])
ON DELETE CASCADE ON UPDATE CASCADE;
GO
