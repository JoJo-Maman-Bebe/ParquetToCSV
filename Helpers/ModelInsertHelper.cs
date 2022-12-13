using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FileReaderAPI.Models;
using System.Reflection;


namespace FileReaderAPI.Helpers
{
	public class ModelInsertHelper


	{

		public void RunSPForModel(object obj, string spName)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");


			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure(spName, connection);

				PropertyInfo[] properties = obj.GetType().GetProperties();
				foreach (PropertyInfo p in properties)
				{
					var fieldval = p.GetValue(obj);
					if (fieldval != null)
					{
						command.Parameters.Add(new SqlParameter("@" + p.Name, p.GetValue(obj)));
					}
					else
					{
						command.Parameters.Add(new SqlParameter("@" + p.Name, ""));
					}


				}
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}



		public void InsertDgBranch(DgBranchModel dgBranchModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");
			

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertDgBranch", connection);

				PropertyInfo[] properties = dgBranchModel.GetType().GetProperties();
				foreach (PropertyInfo p in properties)
				{
					var fieldval = p.GetValue(dgBranchModel);
					if(fieldval != null)
					{
						command.Parameters.Add(new SqlParameter("@" + p.Name, p.GetValue(dgBranchModel)));
					}
					else
					{
						command.Parameters.Add(new SqlParameter("@" + p.Name,""));
					}
					

				}
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}
		public void InsertDgArea(DgAreaModel dgAreaModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertDgArea", connection);
				command.Parameters.Add(new SqlParameter("@AreaCode", dgAreaModel.AreaCode));
				command.Parameters.Add(new SqlParameter("@AreaDesc", dgAreaModel.AreaDesc));
				command.Parameters.Add(new SqlParameter("@RegionCode", dgAreaModel.RegionCode));
				command.Parameters.Add(new SqlParameter("@AreaManager", dgAreaModel.AreaManager));
				command.Parameters.Add(new SqlParameter("@PartnerCode", dgAreaModel.PartnerCode));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertItemOptionBarcode(ItemOptionBarcodeModel itemOptionBarcodeModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertItemOptionBarcode", connection);
				command.Parameters.Add(new SqlParameter("@RowID", itemOptionBarcodeModel.RowID));
				command.Parameters.Add(new SqlParameter("@ItemNumber", itemOptionBarcodeModel.ItemNumber));
				command.Parameters.Add(new SqlParameter("@OptionNumber", itemOptionBarcodeModel.OptionNumber));
				command.Parameters.Add(new SqlParameter("@IsDeleted", itemOptionBarcodeModel.IsDeleted));
				command.Parameters.Add(new SqlParameter("@FulfilCoArticleId", itemOptionBarcodeModel.FulfilCoArticleId));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}
		public void InsertTenders(TenderModel tenderModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertTenders", connection);
				command.Parameters.Add(new SqlParameter("@TenderNo", tenderModel.TenderNo));
				command.Parameters.Add(new SqlParameter("@TransNo", tenderModel.TransNo));
				command.Parameters.Add(new SqlParameter("@TenderType", tenderModel.TenderType));
				command.Parameters.Add(new SqlParameter("@TenderAmount", tenderModel.TenderAmount));
				command.Parameters.Add(new SqlParameter("@TenderAmountBc", tenderModel.TenderAmountBc));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}
		public void InsertTransactions(TransactionModel transactionModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertTransactions", connection);
				command.Parameters.Add(new SqlParameter("@TransNo", transactionModel.TransNo));
				command.Parameters.Add(new SqlParameter("@TransID", transactionModel.TransID));
				command.Parameters.Add(new SqlParameter("@NoItems", transactionModel.NoItems));
				command.Parameters.Add(new SqlParameter("@CompanyInd", transactionModel.CompanyInd));
				command.Parameters.Add(new SqlParameter("@BranchCode", transactionModel.BranchCode));
				command.Parameters.Add(new SqlParameter("@PcNumber", transactionModel.PcNumber));
				command.Parameters.Add(new SqlParameter("@TillNumber", transactionModel.TillNumber));
				command.Parameters.Add(new SqlParameter("@TransDate", transactionModel.TransDate));
				command.Parameters.Add(new SqlParameter("@TransTime", transactionModel.TransTime));
				command.Parameters.Add(new SqlParameter("@SalePerson", transactionModel.SalesPerson));
				command.Parameters.Add(new SqlParameter("@SalesAsstNo", transactionModel.SalesAsstNo));
				command.Parameters.Add(new SqlParameter("@VoidIndicator", transactionModel.VoidIndicator));
				command.Parameters.Add(new SqlParameter("@TaxFreeIndicator", transactionModel.TaxFreeIndicator));
				command.Parameters.Add(new SqlParameter("@TransSaleValue", transactionModel.TransSaleValue));
				command.Parameters.Add(new SqlParameter("@TransSaleValueBc", transactionModel.TransSaleValueBc));
				command.Parameters.Add(new SqlParameter("@TransRealValue", transactionModel.TransRealValue));
				command.Parameters.Add(new SqlParameter("@TransRealValueBc", transactionModel.TransRealValueBc));
				command.Parameters.Add(new SqlParameter("@TransRevenue", transactionModel.TransRevenue));
				command.Parameters.Add(new SqlParameter("@TransRevenueBc", transactionModel.TransRevenueBc));
				command.Parameters.Add(new SqlParameter("@NoSaleReason", transactionModel.NoSaleReason));
				command.Parameters.Add(new SqlParameter("@TbvFlag", transactionModel.TbvFlag));
				command.Parameters.Add(new SqlParameter("@PartnerCode", transactionModel.PartnerCode));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}
		public void InsertStkMovement(StkMovementModel stkMovementModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertStkMovements", connection);
				command.Parameters.Add(new SqlParameter("@SeqNo", stkMovementModel.SeqNo));
				command.Parameters.Add(new SqlParameter("@RecordType", stkMovementModel.RecordType));
				command.Parameters.Add(new SqlParameter("@OwningCompany", stkMovementModel.OwningCompany));
				command.Parameters.Add(new SqlParameter("@CompanyChain", stkMovementModel.CompanyChain));
				command.Parameters.Add(new SqlParameter("@ProductGroup", stkMovementModel.ProductGroup));
				command.Parameters.Add(new SqlParameter("@ItemNumber", stkMovementModel.ItemNumber));
				command.Parameters.Add(new SqlParameter("@OptionNumber", stkMovementModel.OptionNumber));
				command.Parameters.Add(new SqlParameter("@ReasonCode", stkMovementModel.ReasonCode));
				command.Parameters.Add(new SqlParameter("@StockType", stkMovementModel.StockType));
				command.Parameters.Add(new SqlParameter("@DateProcessed", stkMovementModel.DateProcessed));
				command.Parameters.Add(new SqlParameter("@TransactionDate", stkMovementModel.TransactionDate));
				command.Parameters.Add(new SqlParameter("@Quantity", stkMovementModel.Quantity));
				command.Parameters.Add(new SqlParameter("@CostValue", stkMovementModel.CostValue));
				command.Parameters.Add(new SqlParameter("@RetailValue", stkMovementModel.RetailValue));
				command.Parameters.Add(new SqlParameter("@BranchCode", stkMovementModel.BranchCode));
				command.Parameters.Add(new SqlParameter("@TransferNoteNo", stkMovementModel.TransferNoteNo));
				command.Parameters.Add(new SqlParameter("@ToFromBranch", stkMovementModel.ToFromBranch));
				command.Parameters.Add(new SqlParameter("@WarehouseCode", stkMovementModel.WarehouseCode));
				command.Parameters.Add(new SqlParameter("@LocationCode", stkMovementModel.LocationCode));
				command.Parameters.Add(new SqlParameter("@PickingListNo", stkMovementModel.PickingListNo));
				command.Parameters.Add(new SqlParameter("@ConsignmentNo", stkMovementModel.ConsignmentNo));
				command.Parameters.Add(new SqlParameter("@BoxSetNo", stkMovementModel.BoxsetNo));
				command.Parameters.Add(new SqlParameter("@CustOrderNo", stkMovementModel.CustOrderNo));
				command.Parameters.Add(new SqlParameter("@WrdNumber", stkMovementModel.WrdNumber));
				command.Parameters.Add(new SqlParameter("@ActualSalesVal", stkMovementModel.ActualSalesValue));
				command.Parameters.Add(new SqlParameter("@TransactionNo", stkMovementModel.TransactionNo));
				command.Parameters.Add(new SqlParameter("@DocumentNo", stkMovementModel.DocumentNo));
				command.Parameters.Add(new SqlParameter("@ToFromLocation", stkMovementModel.ToFromLocation));
				command.Parameters.Add(new SqlParameter("@BulletinNo", stkMovementModel.BulletinNo));
				command.Parameters.Add(new SqlParameter("@CustOrderInd", stkMovementModel.CustOrderInd));
				command.Parameters.Add(new SqlParameter("@DespatchNoteNo", stkMovementModel.DespatchNoteNo));
				command.Parameters.Add(new SqlParameter("@PickTypeInd", stkMovementModel.PickTypeInd));
				command.Parameters.Add(new SqlParameter("@PickMethod", stkMovementModel.PickMethod));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertStocktake(StocktakeModel stocktakeModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertStockTake", connection);
				command.Parameters.Add(new SqlParameter("@Branch", stocktakeModel.Branch));
				command.Parameters.Add(new SqlParameter("@FrozenGroup", stocktakeModel.FrozenGroup));
				command.Parameters.Add(new SqlParameter("@Item", stocktakeModel.Item));
				command.Parameters.Add(new SqlParameter("@Option", stocktakeModel.Option));
				command.Parameters.Add(new SqlParameter("@Page", stocktakeModel.Page));
				command.Parameters.Add(new SqlParameter("@BookQty", stocktakeModel.BookQty));
				command.Parameters.Add(new SqlParameter("@ActualQty", stocktakeModel.ActualQty));
				command.Parameters.Add(new SqlParameter("@Group", stocktakeModel.Group));
				command.Parameters.Add(new SqlParameter("@Status", stocktakeModel.Status));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void Insert740Transaction (Warehouse740TransactionModel warehouse740TransactionModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("Insert740Transactions", connection);
				command.Parameters.Add(new SqlParameter("@RunDate", warehouse740TransactionModel.RunDate));
				command.Parameters.Add(new SqlParameter("@ItemNo", warehouse740TransactionModel.ItemNo));
				command.Parameters.Add(new SqlParameter("@ItemOption", warehouse740TransactionModel.ItemOption));
				command.Parameters.Add(new SqlParameter("@CountryCode", warehouse740TransactionModel.CountryCode));
				command.Parameters.Add(new SqlParameter("@OrdersTakenQty", warehouse740TransactionModel.OrdersTakenQty));
				command.Parameters.Add(new SqlParameter("@OrdersTakenValue", warehouse740TransactionModel.OrdersTakenValue));
				command.Parameters.Add(new SqlParameter("@DespatchesQty", warehouse740TransactionModel.DespatchesQty));
				command.Parameters.Add(new SqlParameter("@ReturnsQty", warehouse740TransactionModel.ReturnsQty));
				command.Parameters.Add(new SqlParameter("@ReturnsValue", warehouse740TransactionModel.ReturnsValue));
				command.Parameters.Add(new SqlParameter("@CancelsQty", warehouse740TransactionModel.CancelsQty));
				command.Parameters.Add(new SqlParameter("@CancelsValue", warehouse740TransactionModel.CancelsValue));
				command.Parameters.Add(new SqlParameter("@VATRate", warehouse740TransactionModel.VATRate));
				command.Parameters.Add(new SqlParameter("@ClientID", warehouse740TransactionModel.ClientID));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertBankingOverShort(BankingOverShortModel bankingOverShortModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertBankingOverShort", connection);
				command.Parameters.Add(new SqlParameter("@CompanyInd", bankingOverShortModel.CompanyInd));
				command.Parameters.Add(new SqlParameter("@Branch", bankingOverShortModel.Branch));
				command.Parameters.Add(new SqlParameter("@BankDate", bankingOverShortModel.BankDate));
				command.Parameters.Add(new SqlParameter("@SalesPerson", bankingOverShortModel.SalesPerson));
				command.Parameters.Add(new SqlParameter("@SalesAsstNo", bankingOverShortModel.SalesAsstNo));
				command.Parameters.Add(new SqlParameter("@BankTime", bankingOverShortModel.Bank));
				command.Parameters.Add(new SqlParameter("@NoSaleReason", bankingOverShortModel.NoSaleReason));
				command.Parameters.Add(new SqlParameter("@TbvFlag", bankingOverShortModel.TbvFlag));
				command.Parameters.Add(new SqlParameter("@TillNo", bankingOverShortModel.TillNo));
				command.Parameters.Add(new SqlParameter("@TenderType", bankingOverShortModel.TenderType));
				command.Parameters.Add(new SqlParameter("@BankAmount", bankingOverShortModel.BankAmount));
				command.Parameters.Add(new SqlParameter("@BankAmountBc", bankingOverShortModel.BankAmountBc));
				command.Parameters.Add(new SqlParameter("@PolledDate", bankingOverShortModel.PolledDate));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertItemPromotion(ItemPromotionModel itemPromotionModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertItemPromotion", connection);
				command.Parameters.Add(new SqlParameter("@SaleItemNo", itemPromotionModel.SaleItemNo));
				command.Parameters.Add(new SqlParameter("@TransNo", itemPromotionModel.TransNo));
				command.Parameters.Add(new SqlParameter("@PromotionCode", itemPromotionModel.PromotionCode));
				command.Parameters.Add(new SqlParameter("@DiscountValue", itemPromotionModel.DiscountValue));
				command.Parameters.Add(new SqlParameter("@DiscountValueBc", itemPromotionModel.DiscountValueBc));
				command.Parameters.Add(new SqlParameter("@DiscountRate", itemPromotionModel.DiscountRate));

				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertStkBalDaily(StkBranchBalDailyModel stkBranchBalDailyModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertStkBalDaily", connection);
				command.Parameters.Add(new SqlParameter("@OpeningDate", stkBranchBalDailyModel.OpeningDate));
				command.Parameters.Add(new SqlParameter("@OwningCompany", stkBranchBalDailyModel.OwningCompany));
				command.Parameters.Add(new SqlParameter("@SeasonCode", stkBranchBalDailyModel.SeasonCode));
				command.Parameters.Add(new SqlParameter("@BranchCode", stkBranchBalDailyModel.BranchCode));
				command.Parameters.Add(new SqlParameter("@CompanyChain", stkBranchBalDailyModel.CompanyChain));
				command.Parameters.Add(new SqlParameter("@ProductGroup", stkBranchBalDailyModel.ProductGroup));
				command.Parameters.Add(new SqlParameter("@ItemNumber", stkBranchBalDailyModel.ItemNumber));
				command.Parameters.Add(new SqlParameter("@OptionNumber", stkBranchBalDailyModel.OptionNumber));
				command.Parameters.Add(new SqlParameter("@FsUnits", stkBranchBalDailyModel.FsUnits));
				command.Parameters.Add(new SqlParameter("@FsCostVal", stkBranchBalDailyModel.FsCostVal));

				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public void InsertTillCountDetEvent(TillCountDetailsEventModel tillCountDetailsEventModel)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("NextReportDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertTillCountDetailsEvent", connection);
				command.Parameters.Add(new SqlParameter("@TillCountDetailsEventKey", tillCountDetailsEventModel.TillCountKey));
				command.Parameters.Add(new SqlParameter("@StoreNumber", tillCountDetailsEventModel.StoreNumber));
				command.Parameters.Add(new SqlParameter("@Date", tillCountDetailsEventModel.Date));
				command.Parameters.Add(new SqlParameter("@Time", tillCountDetailsEventModel.Time));
				command.Parameters.Add(new SqlParameter("@TillNumber", tillCountDetailsEventModel.TillNumber));
				command.Parameters.Add(new SqlParameter("@NumberOfTills", tillCountDetailsEventModel.NumberOfTills));
				command.Parameters.Add(new SqlParameter("@CountedValue", tillCountDetailsEventModel.CountedValue));
				command.Parameters.Add(new SqlParameter("@DiscrepancyAmount", tillCountDetailsEventModel.DiscrepancyAmount));
				command.Parameters.Add(new SqlParameter("@TenderMethodType", tillCountDetailsEventModel.TenderMethodType));
				command.Parameters.Add(new SqlParameter("@CurrencyCode", tillCountDetailsEventModel.CurrencyCode));

				dbHelper.ExecuteCommandNonQuery(command);
			}


		}

		public string GetConnString(string keyName, string dbServer)
		{
			string ConnSuffix = System.Configuration.ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
			string ConnPrefix = new DatabaseHelper().GetDbServer(dbServer);

			return ConnSuffix + ConnPrefix;

		}

		public void InsertErrorLog(string message, string stacktrace, string fileName, string apiName)
		{
			DatabaseHelper dbHelper = new DatabaseHelper();

			string ConnectionString = GetConnString("ApiLogDb", "wh");

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertErrorLog", connection);
				command.Parameters.Add(new SqlParameter("@AssociatedAPI", apiName));
				command.Parameters.Add(new SqlParameter("@DateTime", DateTime.Now));
				command.Parameters.Add(new SqlParameter("@Message", message));
				command.Parameters.Add(new SqlParameter("@Details", "Failed to process file" + fileName));
				command.Parameters.Add(new SqlParameter("@StackTrace", stacktrace));
				dbHelper.ExecuteCommandNonQuery(command);
			}


		}


	}
}