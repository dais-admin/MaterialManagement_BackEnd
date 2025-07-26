select * from Projects
INSERT INTO Projects VALUES('81e98bc3-fa04-4897-90cf-8393ae0c2a52','Test Project-3','PT-3','Test Remarks','admin',null,'2024-09-20 05:03:41.2952492','2024-09-20 05:03:41.2952492',0)

INSERT INTO Projects VALUES ('672fcf3e-470d-42d2-a5a0-349b450b4aff','Test Project-2','PT-2','Test Remark','admin',null,'2024-09-20 05:03:41.2952492','2024-09-20 05:03:41.2952492',0)



select * from Categories
INSERT INTO Categories VALUES('347a2db8-00e5-4382-a61d-394b7bb4fa8e','Catergory XYZ','C-X','Category Remarks','672fcf3e-470d-42d2-a5a0-349b450b4aff','19c32d91-a170-48ae-ab5b-0c52ca9d6c68','admin',null,'2024-09-20 05:03:41.2952492','2024-09-20 05:03:41.2952492',0)
INSERT INTO Categories VALUES('e638df01-2887-4c0f-971e-60bb89d68b8f','Category PQR','C-P','Category Remarks','672fcf3e-470d-42d2-a5a0-349b450b4aff','19c32d91-a170-48ae-ab5b-0c52ca9d6c68','admin',null,'2024-09-20 05:03:41.2952492','2024-09-20 05:03:41.2952492',0)


select * from Regions
INSERT INTO Regions VALUES('477e57be-a906-42d1-a131-af504d72875d','Region XYZ','R-X','Region Remarks','admin',null,'2024-09-20 05:03:41.2952492','2024-09-20 05:03:41.2952492',0)
INSERT INTO Regions VALUES('3c5e822c-773c-4741-a3a0-29d246c61eb5','Region ABC','R-A','Regions Remarks','admin',null,'2024-09-20 06:07:07.3948924','2024-09-20 06:07:07.3948924',0)

select * from Divisions
INSERT INTO Divisions VALUES('a2acd1dc-92b2-4d5c-a5c7-df6cacff47fb','Division XYZ','D-X','Division Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO Divisions VALUES('6a38df6d-40e4-4369-8157-1e36903fb6a7','Division PQR','D-P','Division Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)


select * from LocationOperations
INSERT INTO LocationOperations VALUES('a1fce79d-0de2-4276-9211-3d10e873a5c7','location XYZ',' L-X',0,0,'LocationOperations Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO LocationOperations VALUES('fd593446-066d-4ba7-94b6-c41d2c73bef7','location PQR',' L-P',0,0,'LocationOperations Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)

select * from Suppliers
INSERT INTO Suppliers VALUES('f3071b58-0e94-448f-9487-359ecfdd6bf9','Supplier PQR','Mumbai,India','Good','1234567890','admin@test.com','Suppliers Remarks','','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO Suppliers VALUES('af7bae00-7d80-481d-b54a-191ae1949c75','Supplier PQR','Mumbai,India','Good','1234567890','admin@test.com','Suppliers Remarks','MaterialDocument\SupplierDocuments\MaintenanceReports.xlsx','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
update Suppliers set SupplierDocument='MaterialDocument\SupplierDocuments\MaintenanceReports.xlsx' where id='F3071B58-0E94-448F-9487-359ECFDD6BF9'



select * from Manufacturers
update Manufacturers set ManufacturerName='Manufacturer XYZ' where id='05eb21dc-ad7d-4cb8-afed-18a873118536'
INSERT INTO Manufacturers VALUES('e3e57ef7-9927-4362-b7c2-877023df52a0','Manufacturer PQR','Pune,India','Good Product','','7896541230','admin@test.com','Manufacturer Remarks','','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO Manufacturers VALUES('05eb21dc-ad7d-4cb8-afed-18a873118536','Manufacturer PQR','Pune,India','Good Product','','7896541230','admin@test.com','Manufacturer Remarks','','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)

select * from ServiceProviders
update ServiceProviders set ServiceProviderName='Provider PQR' where id='99b84b49-31ee-4c90-9257-a8d70211e6a8'
INSERT INTO ServiceProviders VALUES('7d30a539-19b3-4900-99cf-1d3b8805b9a8','Provider XYZ','Pune,India','5632147896','admin@test.com','ServiceProvider Remarks','','05eb21dc-ad7d-4cb8-afed-18a873118536','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO ServiceProviders VALUES('99b84b49-31ee-4c90-9257-a8d70211e6a8','Provider XYZ','Pune,India','5632147896','admin@test.com','ServiceProvider Remarks','','05eb21dc-ad7d-4cb8-afed-18a873118536','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)

select *  from DocumentTypes
INSERT INTO DocumentTypes VALUES('2281bd67-27c0-4bf6-a94b-23979dd1254d','Maintanance Document','DocumentTypes Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)
INSERT INTO DocumentTypes VALUES('bb38107e-d175-4359-ab02-97bec8ac8cab','Warranty Card','DocumentTypes Remarks','admin',null,'2024-09-19 11:14:44.6083086','2024-09-19 11:14:44.6083086',0)