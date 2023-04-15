using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using FileReaderAPI.NextWebService;
using FileReaderAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FileReaderAPI.Helpers
{

    public class IntegrationHelper
    {
        private ICredentials credentials = new NetworkCredential(new ConfigurationHelper().GetAppSettingsValue("navConnUser"), new ConfigurationHelper().GetAppSettingsValue("navConnPassword"));
        private DatabaseHelper databaseHelper = new DatabaseHelper();

        List<NavTransactionHeaderModel> navTransactionHeaderModels = new List<NavTransactionHeaderModel>();
		List<NavPaymentEntryModel> navPaymentEntryModels = new List<NavPaymentEntryModel>();
		List<NavSalesEntryModel> navSalesEntryModels = new List<NavSalesEntryModel>();
        List<int> navErroredTransactions = new List<int>();
        List<NavChargesModel> navChargesModels = new List<NavChargesModel>();
        List<StatementModel> statementModels = new List<StatementModel>();

        private int currentTransactionNo;
        private string currentStoreNo;
        public void InsertTransactionHeader(NavTransactionHeaderModel navTransactionHeaderModel, NextTransactionWS pnextTransactionWS)
        {
            currentStoreNo = navTransactionHeaderModel.BranchCode;
            currentTransactionNo = Convert.ToInt32(navTransactionHeaderModel.ReceiptNumber);

 
            

                pnextTransactionWS.InsertNextTransactionHeader(
                                                navTransactionHeaderModel.ReceiptNumber,
                                                navTransactionHeaderModel.BranchCode,
                                                navTransactionHeaderModel.StaffID,
                                                navTransactionHeaderModel.Date,
                                                navTransactionHeaderModel.Time,
                                                navTransactionHeaderModel.SaleValue,
                                                0,
                                                navTransactionHeaderModel.RealValue);
            
        }




        public void InsertNavCharges(NavChargesModel navChargesModel,NextTransactionWS pnextTransactionWS)
        {



            pnextTransactionWS.InsertNextCharges(
                                                navChargesModel.TransNo,
                                                navChargesModel.BranchCode,
                                                navChargesModel.TransDate,
                                                navChargesModel.TransTime,
                                                navChargesModel.Amount,
                                                navChargesModel.SundryCode);
        }


        public void InsertSalesEntries(NavSalesEntryModel navSalesEntryModel, NextTransactionWS pnextTransactionWS)
        {

                pnextTransactionWS.InsertNextSalesEntries(
                                                    navSalesEntryModel.ReceiptNumber,
                                                    navSalesEntryModel.BranchCode,
                                                    navSalesEntryModel.SalesPerson,
                                                    navSalesEntryModel.Date,
                                                    navSalesEntryModel.Time,
                                                    navSalesEntryModel.SaleValue,
                                                    0,
                                                    navSalesEntryModel.SaleValue,
                                                    navSalesEntryModel.Quantity,
                                                    navSalesEntryModel.NextItemOption,
                                                    navSalesEntryModel.SaleItemNo,
                                                    navSalesEntryModel.VatPercentage);




        }

        public void InsertPaymentEntries(NavPaymentEntryModel navPaymentEntryModel, NextTransactionWS pnextTransactionWS)
        {

                pnextTransactionWS.InsertNextPaymentEntries(
                                                   navPaymentEntryModel.TenderType,
                                                   navPaymentEntryModel.TenderAmount,
                                                   navPaymentEntryModel.AmountInCurrency,
                                                   navPaymentEntryModel.Date,
                                                   navPaymentEntryModel.Time,
                                                   navPaymentEntryModel.SalesPerson,
                                                   navPaymentEntryModel.StoreNo,
                                                   navPaymentEntryModel.ReceiptNo,
                                                   navPaymentEntryModel.SaleItemNo,
                                                   navPaymentEntryModel.NavCardNo);


        }


        public void CreateNextStatements()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetNextStatements", connection);

                using (var reader = dbHelper.ExecuteCommandReader(command))
                {
                    while (reader.Read())
                    {
                        statementModels.Add(GetStatementModel(reader));
                    }

                }

                using (var nextTransactionWS = new NextTransactionWS { Credentials = new NetworkCredential(new ConfigurationHelper().GetAppSettingsValue("navConnUser"), new ConfigurationHelper().GetAppSettingsValue("navConnPassword")) })
                {
                    for (int i = 0; i < statementModels.Count; i++)
                        nextTransactionWS.CreateNextStatement(statementModels[i].Date, statementModels[i].Store, statementModels[i].Bank);
                }
                }

        }
        public void InsertNextTransactions()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("InsertNextTransactionStatus", connection);
                dbHelper.ExecuteCommandReader(command);


            }
            InsertNavStatementsFromNext();
        }

        public void InsertNavStatementsFromNext()
		{


			DatabaseHelper dbHelper = new DatabaseHelper();



            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
			{

				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetNextTransactionHeader", connection);
				using (var reader = dbHelper.ExecuteCommandReader(command))
				{
					while (reader.Read())
					{
						navTransactionHeaderModels.Add(GetNavTransactionHeaderModel(reader));
					}
                }
            }



            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetNextSalesEntry", connection);

                using (var reader = dbHelper.ExecuteCommandReader(command))
                {
                    while (reader.Read())
                    {
                        navSalesEntryModels.Add(GetNavSalesEntryModel(reader));
                    }

                }
            }


            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetNextPaymentEntry", connection);
                using (var reader = dbHelper.ExecuteCommandReader(command))
                {
                    while (reader.Read())
                    {
                        navPaymentEntryModels.Add(GetNavPaymentEntryModel(reader));
                    }
                }

            }



            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
            {

                SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetNavCharges", connection);
                using (var reader = dbHelper.ExecuteCommandReader(command))
                {
                    while (reader.Read())
                    {
                        navChargesModels.Add(GetNavChargesModel(reader));
                    }
                }
            }

            using (var nextTransactionWS = new NextTransactionWS { Credentials = new NetworkCredential(new ConfigurationHelper().GetAppSettingsValue("navConnUser"), new ConfigurationHelper().GetAppSettingsValue("navConnPassword")) })
            {
                for (int i = 0; i < navTransactionHeaderModels.Count; i++)
                {
                    currentTransactionNo = Convert.ToInt32(navTransactionHeaderModels[i].ReceiptNumber);
                    currentStoreNo = navTransactionHeaderModels[i].BranchCode;

                    try
                    {



                        InsertTransactionHeader(navTransactionHeaderModels[i], nextTransactionWS);
                        ////Thread.Sleep(1000);
                        GenerateNavTransStatusLog(Convert.ToInt32(navTransactionHeaderModels[i].ReceiptNumber), Convert.ToInt32(navTransactionHeaderModels[i].BranchCode), 1, 0, 0, 0, 0);

                    }
                    catch (Exception e)
                    {
                        string innerExceptionMessage = e.InnerException != null ? e.InnerException.Message : "";
                        GenerateErrorLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), e.Message, "Transaction Header");
                    }






                }






                for (int i = 0; i < navSalesEntryModels.Count; i++)
                {
                    currentTransactionNo = Convert.ToInt32(navSalesEntryModels[i].ReceiptNumber);
                    currentStoreNo = navSalesEntryModels[i].BranchCode;
                    try
                    {

                        InsertSalesEntries(navSalesEntryModels[i], nextTransactionWS);
                        //Thread.Sleep(1000);
                        GenerateNavTransStatusLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), 0, 1, 0, 0, 0);

                    }
                    catch (Exception e)
                    {
                        string innerExceptionMessage = e.InnerException != null ? e.InnerException.Message : "";
                        GenerateErrorLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), e.Message, "SalesEntry");
                    }


                }





                for (int i = 0; i < navPaymentEntryModels.Count; i++)
                {
                    currentTransactionNo = Convert.ToInt32(navPaymentEntryModels[i].ReceiptNo);
                    currentStoreNo = navPaymentEntryModels[i].StoreNo;

                    try
                    {

                        InsertPaymentEntries(navPaymentEntryModels[i], nextTransactionWS);
                        //Thread.Sleep(1000);
                        GenerateNavTransStatusLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), 1, 1, 1, 0, 0);


                    }
                    catch (Exception e)
                    {
                        string innerExceptionMessage = e.InnerException != null ? e.InnerException.Message : "";
                        GenerateErrorLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), e.Message, "PaymentEntry");
                    }

                }




                for (int i = 0; i < navChargesModels.Count; i++)
                {
                    try
                    {
                        InsertNavCharges(navChargesModels[i], nextTransactionWS);
                        //Thread.Sleep(1000);
                        GenerateNavTransStatusLog(Convert.ToInt32(navChargesModels[i].TransNo), Convert.ToInt32(navChargesModels[i].BranchCode), 1, 1, 1, 1, 0);



                    }
                    catch (Exception e)
                    {
                        string innerExceptionMessage = e.InnerException != null ? e.InnerException.Message : "";
                        GenerateErrorLog(currentTransactionNo, Convert.ToInt32(currentStoreNo), e.Message, "NavCharge");
                    }


                }

            }
            







        }

        private NavTransactionHeaderModel GetNavTransactionHeaderModel(IDataReader reader)
		{
			return new NavTransactionHeaderModel()
			{
				ReceiptNumber = Convert.ToString(reader["TransNo"]),
				BranchCode = Convert.ToString(reader["BranchCode"]),
				StaffID = Convert.ToString(reader["SalesPerson"]),
				Date = Convert.ToString(reader["TransDate"]),
				Time = Convert.ToString(reader["TransTime"]),
				SaleValue = Convert.ToDecimal(reader["TransSaleValueBc"]),
				RealValue = Convert.ToDecimal(reader["TransRealValueBc"]),
				VoidIndicator = Convert.ToChar(reader["EntryStatus"])


			};

		}

        private NavChargesModel GetNavChargesModel(IDataReader reader)
        {
            return new NavChargesModel()
            {
                TransNo = Convert.ToString(reader["TransNo"]),
                BranchCode = Convert.ToString(reader["BranchCode"]),
                SundryCode = Convert.ToString(reader["SundryCode"]),
                TransDate = Convert.ToString(reader["TransDate"]),
                TransTime = Convert.ToString(reader["TransTime"]),
                Amount = Convert.ToDecimal(reader["SundryValueBc"])


            };

        }


        private NavSalesEntryModel GetNavSalesEntryModel(IDataReader reader)
		{
			return new NavSalesEntryModel()
			{
                

                ReceiptNumber = Convert.ToString(reader["TransNo"]),
				NextItemOption = Convert.ToString(reader["NextItemOption"]),
				SaleItemNo = Convert.ToInt32(reader["SaleItemNo"]),
				Date = Convert.ToString(reader["TransDate"]),
				Quantity = Convert.ToInt32(reader["Quantity"]),
				SaleValue = Convert.ToDecimal(reader["SaleValueBc"]),
				SalesPerson = Convert.ToString(reader["SalesPerson"]),
				Time = Convert.ToString(reader["TransTime"]),
				ScannedKeyInd = Convert.ToString(reader["ScannedKeyedInd"]),
				PromoIndicator = Convert.ToBoolean(reader["PromoIndicator"]),
				DiscountIndicator = Convert.ToBoolean(reader["DiscountIndicator"]),
				BranchCode = Convert.ToString(reader["BranchCode"]),
                VatPercentage = Convert.ToDecimal(reader["VatRate"])




            };

		}

		private NavPaymentEntryModel GetNavPaymentEntryModel(IDataReader reader)
		{
			return new NavPaymentEntryModel()
			{
				TenderType = Convert.ToString(reader["TenderDesc"]),
				TenderAmount = Convert.ToDecimal(reader["TenderAmount"]),
				AmountInCurrency = Convert.ToDecimal(reader["AmountInCurrency"]),
				Date = Convert.ToString(reader["TransDate"]),
				Time = Convert.ToString(reader["TransTime"]),
				SalesPerson = Convert.ToString(reader["SalesAsstNo"]),
				StoreNo = Convert.ToString(reader["BranchCode"]),
				ReceiptNo = Convert.ToString(reader["TransNo"]),
                SaleItemNo = Convert.ToInt32(reader["SaleItemNo"]),
                NavCardNo = Convert.ToString(reader["NavCardCode"])

			};

		}

        private StatementModel GetStatementModel(IDataReader reader)
        {
            return new StatementModel()
            {
                Store = Convert.ToInt32(reader["Store"]),
                Date = Convert.ToString(reader["Date"]),
                Bank = Convert.ToDecimal(reader["Bank"]),

            };

        }



        public void GenerateErrorLog(int transNo, int branchCode, string message, string innermessage)
        {
            TransactionErrorLogModel transactionErrorLogModel = new TransactionErrorLogModel()
            {
                TransactionNo = transNo,
                BranchCode = branchCode,
                ExceptionMessage = message,
                InnerExceptionMessage = innermessage
            };

            ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
            modelInsertHelper.RunSPForModel(transactionErrorLogModel, "InsertTransactionErrorLog");
        }

        public void GenerateNavTransStatusLog(int transNo, int branchCode, int headerInsert, int salesEntryInsert, int paymentEntryInsert,int navChargeAdded, int bankCountAdded)
        {
            NavTransactionStatusModel navTransactionStatusModel = new NavTransactionStatusModel()
            {
                TransactionNumber = transNo,
                BranchCode = branchCode,
                HeaderInserted = headerInsert,
                SalesEntryInserted = salesEntryInsert,
                PaymentEntryInserted = paymentEntryInsert,
                BankCountAdded = bankCountAdded,
                NavChargeAdded = navChargeAdded

            };

            ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
            modelInsertHelper.RunSPForModel(navTransactionStatusModel, "InsertNavTransactionStatus");
        }




    }


}