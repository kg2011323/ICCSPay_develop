/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [MobilePayDB].[dbo].[StationOrder]
  order by BuyTime desc
  
--SELECT *
--  FROM [MobilePayDB].[dbo].[WebOrderVoucher]
  
--SELECT *
--  FROM [MobilePayDB].[dbo].[VoucherList]
--  where IsUsed = 1
  
--SELECT *
--  FROM [MobilePayDB].[dbo].[WebOrderRefund]


