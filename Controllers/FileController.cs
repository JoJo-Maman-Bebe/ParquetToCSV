using System;
using System.Collections.Generic;
using System.Web.Http;
using ChoETL;
using System.IO;
using FileReaderAPI.Models;
using FileReaderAPI.Helpers;
using System.Data.SqlClient;
using System.Data;

namespace FileReaderAPI.Controllers
{

	

	[RoutePrefix("api/file")]
	public class FileController : ApiController
	{
		public string storeNumber;
		public string baseFolder = new ConfigurationHelper().GetAppSettingsValue("parquetFileBaseFolder");
		public ModelInsertHelper modelInsertHelper = new ModelInsertHelper();

		[HttpGet]
		[Route("InsertAllTransactionData")]

		public IHttpActionResult InsertAllTransactionData()
        {
            try
            {
				ConvertSaleItem();
				ConvertTransaction();
				ConvertTender();
				ConvertLuTender();
				ConvertBankingCitEvent();
				ConvertTillCountDetEvent();
				ConvertBankCount();
				ConvertSundryBranch();
				return Ok("Success");
			}
			catch(Exception e)
            {
				return BadRequest(e.Message);
            }
			
        }
		[HttpGet]
		[Route("InsertNextStatements")]

		public IHttpActionResult InsertNextStatementsToNav()
        {
			IntegrationHelper integrationHelper = new IntegrationHelper();

			integrationHelper.InsertNextTransactions();
			integrationHelper.CreateNextStatements();

			return Ok("Success");
        }

		[HttpGet]
		[Route("GetNewFiles/{id}")]

		public IHttpActionResult DownloadNewParquetFiles()
		{
			try
			{
				//sastoken = 42M8Q~JmFL6GOI.~9cODIgBTyVguzhyOE4Sd7cT8

				AzureHelper azureHelper = new AzureHelper();
				azureHelper.GetNewParquetFiles();

			}
			catch (Exception e)
			{
				return BadRequest(e.Message);

			}
			return Ok("Ok");
		}


		[HttpGet]
		[Route("GetFiles/{id}")]

		public  IHttpActionResult DownloadParquetFiles()
		{
            try
            {
				//rQh8Q~MPAGFed_mTDIwCKvv_yggk93Is~PC2baQT
				//42M8Q~JmFL6GOI.~9cODIgBTyVguzhyOE4Sd7cT8
							//string containerUrl = "https://jmblandingdev.blob.core.windows.net/inbound";
							//string sasToken = "sv=2021-06-08&ss=bfqt&srt=sco&sp=rlx&se=2023-10-24T18:07:54Z&st=2022-10-24T10:07:54Z&sip=188.227.240.140&spr=https&sig=pnIEu9dBNEveH6IMV98YDcpKnWKTEb%2BW73amLyOebXo%3D";

							AzureHelper azureHelper = new AzureHelper();
			azureHelper.GetFirstParquetFile("https://jmblandingdev.blob.core.windows.net/inbound", "sv=2021-06-08&ss=bfqt&srt=sco&sp=rlx&se=2023-10-24T18:07:54Z&st=2022-10-24T10:07:54Z&sip=188.227.240.140&spr=https&sig=pnIEu9dBNEveH6IMV98YDcpKnWKTEb%2BW73amLyOebXo%3D");

			}
			catch(Exception e)
            {
				return BadRequest(e.Message);

            }
			return Ok("Ok");
		}

		[HttpGet]
		[Route("MemberContacts")]
		public IHttpActionResult CreateMemberContactParquet()
		{

			List<MemberContactModel> memberContactList = new List<MemberContactModel>();
			DatabaseHelper dbHelper = new DatabaseHelper();

			using (SqlConnection connection = new SqlConnection(dbHelper.GetConnString("NextReportDb", "wh")))
			{

				SqlCommand command = dbHelper.CommandGeneratorStoredProcedure("GetMemberContacts", connection);
				using (var reader = dbHelper.ExecuteCommandReader(command))
				{
					while (reader.Read())
					{
						memberContactList.Add(GetMemberContactData(reader));
					}

				}

				using (var parser = new ChoParquetWriter("C:\\JoJo Maman Bébé\\NEXT\\MemberContact2022.parquet"))
				{
					
					parser.Write(memberContactList);
				}
			}

			return Ok("Success");
		}

		private MemberContactModel GetMemberContactData(IDataReader reader)
		{
			return new MemberContactModel()
			{
				AccountNumber = Convert.ToString(reader["Account No_"]),
				FirstName = Convert.ToString(reader["First Name"]),
				Surname = Convert.ToString(reader["Surname"]),
				Address = Convert.ToString(reader["Address"]),
				Address2 = Convert.ToString(reader["Address 2"]),
				City = Convert.ToString(reader["City"]),
				PostCode = Convert.ToString(reader["Post Code"]),
				Email = Convert.ToString(reader["E-Mail"]),
				PhoneNumber = Convert.ToString(reader["Phone No_"]),
				MobilePhoneNumber = Convert.ToString(reader["Mobile Phone No_"]),
				Country = Convert.ToString(reader["Country"]),
				ContactViaEmail = Convert.ToBoolean(reader["Contact Via Email"]),
				ContactViaPhone = Convert.ToBoolean(reader["Contact Via Telephone"]),
				ContactViaPost = Convert.ToBoolean(reader["Contact Via Post"]),
				BlockAllContact = Convert.ToBoolean(reader["Block All Contact"])



			};

		}

		[HttpGet]
		[Route("740Transactions")]

		public IHttpActionResult Convert740Transactions()
		{

			

		try
			{


		
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"Warehouse740Transactions");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach(FileInfo file in files)
				{
					try
					{


					
					var reader = new ChoParquetReader(file.FullName);
					dynamic rec;

					while ((rec = reader.Read()) != null )
					{

						Warehouse740TransactionModel warehouse740TransactionModel = new Warehouse740TransactionModel()
						{
							RunDate = Convert.ToString(rec.RunDate),
							ItemNo = Convert.ToString(rec.ItemNo),
							ItemOption = Convert.ToString(rec.ItemOption),
							CountryCode = Convert.ToString(rec.CountryCode),
							OrdersTakenQty = Convert.ToInt32(rec.OrdersTakenQty),
							OrdersTakenValue = Convert.ToDecimal(rec.OrdersTakenValue),
							DespatchesQty = Convert.ToInt32(rec.DespatchesQty),
							ReturnsQty = Convert.ToInt32(rec.ReturnsQty),
							ReturnsValue = Convert.ToDecimal(rec.ReturnsValue),
							CancelsQty = Convert.ToInt32(rec.CancelsQty),
							CancelsValue = Convert.ToDecimal(rec.CancelsValue),
							VATRate = Convert.ToInt32(rec.VATRate),
							ClientID = Convert.ToString(rec.ClientID)
						};

						modelInsertHelper.Insert740Transaction(warehouse740TransactionModel);
					}
					
					}
					catch(Exception ex)
					{
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "740 Transactions");
						GenerateActivityLog("740Transactions", "Error");
						file.CopyTo(baseFolder + @"Warehouse740Transactions\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("740Transactions", "Error");
				return BadRequest("Errored with " + ex.Message);

			}

			
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"Warehouse740Transactions");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"Warehouse740Transactions\Archive\" + file.Name);

			}

			GenerateActivityLog("740Transactions", "Success");
			return Ok("Successful");
				
			}





		[HttpGet]
		[Route("BankingOverShort")]

		public IHttpActionResult ConvertBankingOverShort()
		{



			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"BankingOverShort");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null )
						{

							BankingOverShortModel bankingOverShortModel = new BankingOverShortModel()
							{
									CompanyInd = Convert.ToString(rec.company_ind),
									Branch = Convert.ToString(rec.branch),
									BankDate = Convert.ToString(rec.bank_date),
									SalesPerson = Convert.ToString(rec.salesperson),
									SalesAsstNo = Convert.ToInt32(rec.sales_asst_no),
									Bank = Convert.ToString(rec.bank_time),
									NoSaleReason = Convert.ToString(rec.no_sale_reason),
									TbvFlag = Convert.ToString(rec.tbv_flag),
									TillNo = Convert.ToInt32(rec.till_no),
									TenderType = Convert.ToString(rec.tender_type),
									BankAmount = Convert.ToDecimal(rec.bank_amount),
									BankAmountBc = Convert.ToDecimal(rec.bank_amount_bc),
									PolledDate = Convert.ToString(rec.polled_date)
							};
							
							modelInsertHelper.InsertBankingOverShort(bankingOverShortModel);
						}

					}
					catch (Exception ex)
					{
						GenerateActivityLog("BankingOverShort", "Error");
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Banking Over Short");

						file.CopyTo(baseFolder + @"BankingOverShort\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("BankingOverShort", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"BankingOverShort");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"BankingOverShort\Archive\" + file.Name);

			}
			GenerateActivityLog("BankingOverShort", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("ItemPromotion")]

		public IHttpActionResult ConvertItemPromotion()
		{



			try
			{


				var reader = new ChoParquetReader();
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"ItemPromotion");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{

							ItemPromotionModel itemPromotionModel = new ItemPromotionModel()
							{
								SaleItemNo = Convert.ToInt32(rec.sale_item_no),
								TransNo = Convert.ToInt32(rec.trans_no),
								PromotionCode = Convert.ToInt32(rec.promotion_code),
								DiscountValue = Convert.ToInt32(rec.discount_value),
								DiscountValueBc = Convert.ToInt32(rec.discount_value_bc),
								DiscountRate = Convert.ToInt32(rec.discount_rate),
							};
							
							modelInsertHelper.InsertItemPromotion(itemPromotionModel);
						}

					}
					catch (Exception ex)
					{
						reader.Dispose();
						GenerateActivityLog("ItemPromotion", "Error");
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Item Promotion");

						file.CopyTo(baseFolder + @"ItemPromotion\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				
				GenerateActivityLog("ItemPromotion", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"ItemPromotion");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"ItemPromotion\Archive\" + file.Name);

			}
			GenerateActivityLog("ItemPromotion", "Success");
			return Ok("Successful");

		}


		
		
		[HttpGet]
		[Route("StkBalDaily")]

		public IHttpActionResult ConvertStkBranchBalDaily()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"StkBranchBalDaily");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							StkBranchBalDailyModel stkBranchBalDailyModel = new StkBranchBalDailyModel()
							{
								OpeningDate = Convert.ToString(rec.opening_date),
								OwningCompany = Convert.ToString(rec.owning_company),
								SeasonCode = Convert.ToString(rec.season_code),
								BranchCode = Convert.ToInt32(rec.branch_code),
								CompanyChain = Convert.ToString(rec.company_chain),
								ProductGroup = Convert.ToString(rec.product_group),
								ItemNumber = Convert.ToString(rec.item_number),
								OptionNumber = Convert.ToString(rec.option_number),
								FsUnits = Convert.ToInt32(rec.fs_units),
								FsCostVal = Convert.ToDecimal(rec.fs_cost_val)
							};
							
							modelInsertHelper.InsertStkBalDaily(stkBranchBalDailyModel);
							
						}

					}
					catch (Exception ex)
					{
						
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Stk Branch Bal Daily");

						file.CopyTo(baseFolder + @"StkBranchBalDaily\Errored\" + file.Name);
						GenerateActivityLog("StkBalDaily", "Error");
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("StkBalDaily", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"StkBranchBalDaily");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				
				file.MoveTo(baseFolder + @"StkBranchBalDaily\Archive\" + file.Name);

			}
			GenerateActivityLog("StkBalDaily", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("TillCountDetailsEvent")]

		public IHttpActionResult ConvertTillCountDetEvent()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"TillCountDetailsEvent");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							TillCountDetailsEventModel tillCountDetailsEventModel = new TillCountDetailsEventModel()
							{
								TillCountKey= Convert.ToInt32(rec.till_count_details_event_key),
								StoreNumber = Convert.ToInt32(rec.store_number),
								Date = Convert.ToString(rec.date),
								Time = Convert.ToString(rec.time),
								TillNumber = Convert.ToInt32(rec.till_number),
								NumberOfTills = Convert.ToInt32(rec.number_of_tills),
								CountedValue = Convert.ToInt32(rec.counted_value),
								DiscrepancyAmount = Convert.ToInt32(rec.discrepancy_amount),
								TenderMethodType = Convert.ToInt32(rec.tender_method_type),
								CurrencyCode = Convert.ToString(rec.currency_code)
							};
							
							modelInsertHelper.InsertTillCountDetEvent(tillCountDetailsEventModel);

						}

					}
					catch (Exception ex)
					{
						GenerateActivityLog("TillCountDetailsEvent", "Error");
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Till Count Details Event");

						file.CopyTo(baseFolder + @"TillCountDetailsEvent\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("TillCountDetailsEvent", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"TillCountDetailsEvent");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"TillCountDetailsEvent\Archive\" + file.Name);

			}
			GenerateActivityLog("TillCountDetailsEvent", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("Stocktake")]

		public IHttpActionResult ConvertStockTake()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"StockTake");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							StocktakeModel stocktakeModel = new StocktakeModel()
							{
								Branch = Convert.ToInt32(rec.branch),
								FrozenGroup = Convert.ToString(rec.frozen_group),
								Item = Convert.ToString(rec.item),
								Option = Convert.ToString(rec.option),
								Page = Convert.ToInt32(rec.page),
								BookQty = Convert.ToInt32(rec.book_qty),
								ActualQty = Convert.ToInt32(rec.actual_qty),
								Group = Convert.ToString(rec.group),
								Status = Convert.ToString(rec.status),
							};
							
							modelInsertHelper.InsertStocktake(stocktakeModel);

						}

					}
					catch (Exception ex)
					{
						GenerateActivityLog("StockTake", "Error");
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Stock Take");

						file.CopyTo(baseFolder + @"StockTake\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("StockTake", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"StockTake");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"StockTake\Archive\" + file.Name);

			}
			GenerateActivityLog("StockTake", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("StkMovements")]

		public IHttpActionResult ConvertStkMovements()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"StkMovements");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							StkMovementModel stkMovementModel = new StkMovementModel()
							{
								SeqNo = Convert.ToInt64(rec.seq_no),
								RecordType = Convert.ToString(rec.record_type),
								OwningCompany = Convert.ToString(rec.owning_company),
								CompanyChain = Convert.ToString(rec.company_chain),
								ProductGroup = Convert.ToString(rec.product_group),
								ItemNumber = Convert.ToString(rec.item_number),
								OptionNumber = Convert.ToString(rec.option_number),
								ReasonCode = Convert.ToString(rec.reason_code),
								StockType = Convert.ToString(rec.stock_type),
								DateProcessed = Convert.ToString(rec.date_processed),
								TransactionDate = Convert.ToString(rec.transaction_date),
								Quantity = Convert.ToInt32(rec.quantity),
								CostValue = Convert.ToDecimal(rec.cost_value),
								RetailValue = Convert.ToDecimal(rec.retail_value),
								BranchCode = Convert.ToInt32(rec.branch_code),
								TransferNoteNo = Convert.ToString(rec.transfer_note_no),
								ToFromBranch = Convert.ToInt32(rec.to_from_branch),
								WarehouseCode = Convert.ToString(rec.warehouse_code),
								LocationCode = Convert.ToString(rec.location_code),
								PickingListNo = Convert.ToString(rec.picking_list_no),
								ConsignmentNo = Convert.ToString(rec.consignment_no),
								BoxsetNo = Convert.ToString(rec.boxset_no),
								CustOrderNo = Convert.ToString(rec.cust_order_no),
								WrdNumber = Convert.ToString(rec.wrd_number),
								ActualSalesValue = Convert.ToDecimal(rec.actual_sales_val),
								TransactionNo = Convert.ToInt32(rec.transaction_no),
								DocumentNo = Convert.ToString(rec.document_no),
								ToFromLocation = Convert.ToString(rec.to_from_location),
								BulletinNo = Convert.ToString(rec.bulletin_no),
								CustOrderInd = Convert.ToString(rec.cust_order_ind),
								DespatchNoteNo = Convert.ToString(rec.despatch_note_no),
								PickTypeInd = Convert.ToString(rec.pick_type_ind),
								PickMethod = Convert.ToString(rec.pick_method),
								RatioPackInd = Convert.ToString(rec.ratio_pack_ind),
								BoxsetSeqNo = Convert.ToString(rec.boxset_seq_no),
								ProductPartnerCode = Convert.ToString(rec.ProductPartnerCode),
								BranchPartnerCode = Convert.ToString(rec.BranchPartnerCode)
							};
							
							modelInsertHelper.InsertStkMovement(stkMovementModel);

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "StkMovements");
						GenerateActivityLog("StkMovements", "Error");
						file.CopyTo(baseFolder + @"StkMovements\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("StkMovements", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"StkMovements");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"StkMovements\Archive\" + file.Name);

			}
			GenerateActivityLog("StkMovements", "Success");
			return Ok("Successful");

		}

		

		[HttpGet]
		[Route("DgBranch")]

		public IHttpActionResult ConvertDgBranch()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"DgBranch");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							DgBranchModel dgBranchModel = new DgBranchModel()
							{
								BranchCode = Convert.ToInt32(rec.branch_code),
								BranchDesc = Convert.ToString(rec.branch_desc),
								AreaCode = Convert.ToInt32(rec.area_code),
								BranchAddress1 = Convert.ToString(rec.branch_address1),
								BranchAddress2 = Convert.ToString(rec.branch_address2),
								BranchAddress3 = Convert.ToString(rec.branch_address3),
								BranchAddress4 = Convert.ToString(rec.branch_address4),
								PostCode = Convert.ToString(rec.post_code),
								Telephone = Convert.ToString(rec.telephone),
								NoTills = Convert.ToInt32(rec.no_tills),
								BranchSqFeet = Convert.ToInt32(rec.branch_sq_feet),
								BranchCoChain = Convert.ToString(rec.branch_co_chain),
								OpeningDate = Convert.ToString(rec.opening_date),
								ClosingDate = Convert.ToString(rec.closing_date),
								CountryCode = Convert.ToString(rec.country_code),
								GlCompanyCode = Convert.ToString(rec.gl_company_code),
								FranchisePartner = Convert.ToString(rec.franchise_partner),
								FranBulkOrRepl = Convert.ToString(rec.fran_bulk_or_repl),
								SatOpeningFlag = Convert.ToString(rec.sat_opening_flag),
								MerSwiaccvisa = Convert.ToString(rec.mer_swiaccvisa),
								MerAmex = Convert.ToString(rec.mer_amex),
								MerDiners = Convert.ToString(rec.mer_diners),
								MerClub24 = Convert.ToString(rec.mer_club24),
								MerTime = Convert.ToString(rec.mer_time),
								MerJcb = Convert.ToString(rec.mer_jcb),
								MerStyle = Convert.ToString(rec.mer_style),
								AlcoholLicense = Convert.ToString(rec.alcohol_license),
								AlOpeningMon = Convert.ToString(rec.al_opening_mon),
								AlOpeningTue = Convert.ToString(rec.al_opening_tue),
								AlOpeningWed = Convert.ToString(rec.al_opening_wed),
								AlOpeningThu = Convert.ToString(rec.al_opening_thu),
								AlOpeningFri = Convert.ToString(rec.al_opening_fri),
								AlClosingMon = Convert.ToString(rec.al_closing_mon),
								AlClosingTue = Convert.ToString(rec.al_closing_tue),
								AlClosingWed = Convert.ToString(rec.al_closing_wed),
								AlClosingThu = Convert.ToString(rec.al_closing_thu),
								AlClosingFri = Convert.ToString(rec.al_closing_fri),
								AlOpeningSat = Convert.ToString(rec.al_opening_sat),
								AlOpeningSun = Convert.ToString(rec.al_opening_sun),
								AlClosingSat = Convert.ToString(rec.al_closing_sat),
								AlClosingSun = Convert.ToString(rec.al_closing_sun),
								BranchType = Convert.ToString(rec.branch_type),
								CompanyInd = Convert.ToInt32(rec.company_ind),
								PartnerCode = Convert.ToString(rec.partner_code)	
							};
							
							modelInsertHelper.RunSPForModel(dgBranchModel, "InsertDgBranch");

						}

					}
					catch (Exception ex)
					{
						GenerateActivityLog("DgBranch", "Error");
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "DgBranch");

						file.CopyTo(baseFolder + @"DgBranch\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("DgBranch", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"DgBranch");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"DgBranch\Archive\" + file.Name);

			}
			GenerateActivityLog("DgBranch", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("BankingCitEvent")]

		public IHttpActionResult ConvertBankingCitEvent()
		{



			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"BankingCitEvent");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							BankingCitEventModel bankingCitEventModel = new BankingCitEventModel()
							{
								MessageID = Convert.ToString(rec.MessageID),
								VersionNo = Convert.ToString(rec.VersionNo),
								StoreNumber = Convert.ToInt32(rec.StoreNumber),
								MessageType = Convert.ToInt32(rec.MessageType),
								BankingDate = Convert.ToString(rec.BankingDate),
								BankSlipNumber = Convert.ToInt32(rec.BankSlipNumber),
								Tender = Convert.ToString(rec.Tender),
								Value = Convert.ToString(rec.Value),
								BagNumber = Convert.ToString(rec.BagNumber),
								ReceiptNumber = Convert.ToString(rec.ReceiptNumber),
								Status = Convert.ToString(rec.Status),
								StoreBankerPayrollNumber = Convert.ToInt64(rec.StoreBankerPayrollNumber),
								StoreBankerCheckerPayrollNumber = Convert.ToInt64(rec.StoreBankerCheckerPayrollNumber),
								MessageActionDate = Convert.ToString(rec.MessageActionDate),
								MessageActionTime = Convert.ToString(rec.MessageActionTime),
								MessageActionCheckPayrollNumber = Convert.ToInt64(rec.MessageActionCheckPayrollNumber),
								MessageActionPayrollNumber = Convert.ToInt64(rec.MessageActionPayrollNumber),
								TenderType = Convert.ToInt32(rec.TenderType),
								CurrencyCode = Convert.ToString(rec.CurrencyCode),
							};
							
							modelInsertHelper.RunSPForModel(bankingCitEventModel, "InsertBankingCitEvents");
						}

					}
					catch (Exception ex)
					{
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Banking CIT Event");
						GenerateActivityLog("BankingCitEvent", "Error");
						file.CopyTo(baseFolder + @"BankingCitEvent\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("BankingCitEvent", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"BankingCitEvent");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"BankingCitEvent\Archive\" + file.Name);

			}
			GenerateActivityLog("BankingCitEvent", "Success");
			return Ok("Successful");

		}

		[HttpGet]
		[Route("GcTransaction")]

		public IHttpActionResult ConvertGcTransaction()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"GcTransaction");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null )
						{


							GcTransactionModel gcTransactionModel = new GcTransactionModel()
							{
								TransNo = Convert.ToInt32(rec.trans_no),
								TransDate = Convert.ToString(rec.trans_date),
								TransTime = Convert.ToInt32(rec.trans_time),
								BranchCode = Convert.ToInt32(rec.branch_code),
								TillNumber = Convert.ToInt32(rec.till_number),
								TransID = Convert.ToString(rec.trans_id),
								PassFailInd = Convert.ToInt32(rec.pass_fail_ind),
								GiftcardID = Convert.ToString(rec.giftcard_id),
								SeqNo = Convert.ToInt32(rec.seqno),
								TransType = Convert.ToInt32(rec.trans_type),
								PolledStatus = Convert.ToInt32(rec.polled_status),
								Amount = Convert.ToDecimal(rec.amount),
								CurrencyID = Convert.ToInt32(rec.currency_id),
								ExchangeRate = Convert.ToDecimal(rec.exchange_rate),
								OperatorID = Convert.ToString(rec.operator_id),
								SupervisorID = Convert.ToString(rec.supervisor_id),
								VoidIndicator = Convert.ToInt32(rec.void_indicator),
								NorFlag = Convert.ToString(rec.nor_flag),
								TransReason = Convert.ToInt32(rec.trans_reason),
								PolledDate = Convert.ToString(rec.polled_date),
								DirectoryAccountNo = Convert.ToString(rec.directory_account_no),
								Comments = Convert.ToString(rec.comments),
								ClientID = Convert.ToString(rec.ClientID),
								PartnerCode = Convert.ToString(rec.PartnerCode)
							};
							
							modelInsertHelper.RunSPForModel(gcTransactionModel, "InsertGcTransaction");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "GcTransaction");
						GenerateActivityLog("GcTransaction", "Error");
						file.CopyTo(baseFolder + @"GcTransaction\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("GcTransaction", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"GcTransaction");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"GcTransaction\Archive\" + file.Name);

			}
			GenerateActivityLog("GcTransaction", "Success");
			return Ok("Successful");

		}


		

		[HttpGet]
		[Route("ItemDiscount")]

		public IHttpActionResult ConvertItemDiscount()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"ItemDiscount");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							ItemDiscountModel itemDiscountModel = new ItemDiscountModel()
							{
								SaleItemNo = Convert.ToInt32(rec.sale_item_no),
								TransNo = Convert.ToInt64(rec.trans_no),
								DiscountValue = Convert.ToInt32(rec.discount_value),
								DiscountValueBc = Convert.ToInt32(rec.discount_value_bc),
								DiscountRate = Convert.ToInt32(rec.discount_rate),
								DiscountCardID = Convert.ToInt32(rec.discount_card_id),
								DiscountCardNo = Convert.ToInt32(rec.discount_card_no),
								DiscountCardVer = Convert.ToInt32(rec.discount_card_ver),
								DiscountKeyWipe = Convert.ToString(rec.discount_key_wipe),
								DisUniformInd = Convert.ToString(rec.dis_uniform_id)
							};
							
							modelInsertHelper.RunSPForModel(itemDiscountModel, "InsertItemDiscount");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "ItemDiscount");
						GenerateActivityLog("ItemDiscount", "Error");
						file.CopyTo(baseFolder + @"ItemDiscount\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("ItemDiscount", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"ItemDiscount");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"ItemDiscount\Archive\" + file.Name);

			}
			GenerateActivityLog("ItemDiscount", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("DGRegion")]

		public IHttpActionResult ConvertDGRegion()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"DGRegion");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							DgRegionModel dgRegionModel = new DgRegionModel()
							{
								RegionCode = Convert.ToInt32(rec.region_code),
								RegionDesc = Convert.ToString(rec.region_desc),
								CountryCode = Convert.ToString(rec.country_code),
								PartnerCode = Convert.ToString(rec.partner_code),
							};
							
							modelInsertHelper.RunSPForModel(dgRegionModel, "InsertDGRegion");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "DGRegion");
						GenerateActivityLog("DGRegion", "Error");
						file.CopyTo(baseFolder + @"DGRegion\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("DGRegion", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"DGRegion");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"DGRegion\Archive\" + file.Name);

			}
			GenerateActivityLog("DGRegion", "Success");
			return Ok("Successful");
		}

		

		[HttpGet]
		[Route("DGArea")]

		public IHttpActionResult ConvertDGArea()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"DGArea");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							DgAreaModel dgAreaModel = new DgAreaModel()
							{
								AreaCode = Convert.ToInt32(rec.area_code),
								AreaDesc = Convert.ToString(rec.area_desc),
								RegionCode = Convert.ToInt32(rec.region_code),
								AreaManager = Convert.ToString(rec.area_manager),
								PartnerCode = Convert.ToString(rec.partner_code)
							};
							
							modelInsertHelper.RunSPForModel(dgAreaModel, "InsertDGArea");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "DGArea");
						GenerateActivityLog("DGArea", "Error");
						file.CopyTo(baseFolder + @"DGArea\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("DGArea", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"DGArea");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"DGArea\Archive\" + file.Name);

			}
			GenerateActivityLog("DGArea", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("Tender")]

		public IHttpActionResult ConvertTender()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"Tender");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							TenderModel tenderModel = new TenderModel()
							{
								TenderNo = Convert.ToInt32(rec.tender_no),
								TransNo = Convert.ToInt64(rec.trans_no),
								TenderType = Convert.ToInt32(rec.tender_type),
								TenderAmount = Convert.ToInt32(rec.tender_amount),
								TenderAmountBc = Convert.ToInt32(rec.tender_amount_bc)

							};
							
							modelInsertHelper.RunSPForModel(tenderModel, "InsertTender");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Tender");
						GenerateActivityLog("Tender", "Error");
						file.CopyTo(baseFolder + @"Tender\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("Tender", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"Tender");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"Tender\Archive\" + file.Name);

			}
			GenerateActivityLog("Tender", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("Transaction")]

		public IHttpActionResult ConvertTransaction()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"Transaction");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{

						

						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							TransactionModel transactionModel = new TransactionModel()
							{
								TransNo = Convert.ToInt64(rec.trans_no),
								TransID = Convert.ToInt32(rec.trans_id),
								NoItems = Convert.ToInt32(rec.no_items),
								CompanyInd = Convert.ToInt32(rec.company_ind),
								BranchCode = Convert.ToInt32(rec.branch_code),
								PcNumber = Convert.ToInt32(rec.pc_number),
								TillNumber = Convert.ToInt32(rec.till_number),
								TransDate = Convert.ToString(rec.trans_date),
								TransTime = Convert.ToInt32(rec.trans_time),
								SalesPerson = Convert.ToInt32(rec.salesperson),
								SalesAsstNo = Convert.ToInt32(rec.sales_asst_no),
								VoidIndicator = Convert.ToString(rec.void_indicator),
								TaxFreeIndicator = Convert.ToString(rec.taxfree_indicator),
								TransSaleValue = Convert.ToInt32(rec.trans_sale_value),
								TransSaleValueBc = Convert.ToInt32(rec.trans_sale_value_bc),
								TransRealValue = Convert.ToInt32(rec.trans_real_value),
								TransRealValueBc = Convert.ToInt32(rec.trans_real_value_bc),
								TransRevenue = Convert.ToInt32(rec.trans_revenue),
								TransRevenueBc = Convert.ToInt32(rec.trans_revenue_bc),
								NoSaleReason = Convert.ToString(rec.no_sale_reason),
								TbvFlag = Convert.ToString(rec.tbvflag),
								PartnerCode = Convert.ToString(rec.partner_code)
							};
							
							modelInsertHelper.RunSPForModel(transactionModel, "InsertTransactions");

						}

					}
					catch (Exception ex)
					{


						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Transaction");
						GenerateActivityLog("Transaction", "Error");
						if(file.Exists)
                        {
							file.CopyTo(baseFolder + @"Transaction\Errored\" + file.Name);
						}
						
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("Transaction", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"Transaction");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				if(file.Exists)
                {
					file.MoveTo(baseFolder + @"Transaction\Archive\" + file.Name);
				}
				

			}
			GenerateActivityLog("Transaction", "Success");
			return Ok("Successful");
		}


		[HttpGet]
		[Route("TenderCredit")]

		public IHttpActionResult ConvertTenderCredit()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"TenderCredit");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							TenderCreditModel TenderCreditModel = new TenderCreditModel()
							{
								TenderNo = Convert.ToInt32(rec.tender_no),
								TransNo = Convert.ToInt64(rec.trans_no),
								TenderAccNo = Convert.ToString(rec.tender_acc_no),
								TenderExp = Convert.ToString(rec.tender_exp),
								TenderAuthCode = Convert.ToString(rec.tender_auth_code),
								KeyWipeInd = Convert.ToString(rec.key_wipe_ind),
								TenderAccVerNo = Convert.ToString(rec.tender_acc_ver_no),
								TenderAmount = Convert.ToInt32(rec.tender_amount),
								TenderAmountBc = Convert.ToInt32(rec.tender_amount_bc),
								AuthMethod = Convert.ToString(rec.auth_method),
								ContactlessFormFactor = Convert.ToString(rec.contactless_form_factor),
								PSPProvider = Convert.ToString(rec.PSPProvider),
								PartnerCode = Convert.ToString(rec.partner_code)

							};
							
							modelInsertHelper.RunSPForModel(TenderCreditModel, "InsertTenderCredit");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "TenderCredit");
						GenerateActivityLog("TenderCredit", "Error");
						file.CopyTo(baseFolder + @"TenderCredit\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("TenderCredit", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"TenderCredit");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"TenderCredit\Archive\" + file.Name);

			}
			GenerateActivityLog("TenderCredit", "Success");
			return Ok("Successful");
		}


		[HttpGet]
		[Route("LuItemPromotion")]

		public IHttpActionResult ConvertLuItemPromotion()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"LuItemPromotion");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							LuItemPromotion luItemPromotion = new LuItemPromotion()
							{
								PromoCode = Convert.ToInt32(rec.tender_no),
								PromoDesc = Convert.ToString(rec.trans_no),


							};
							
							modelInsertHelper.RunSPForModel(luItemPromotion, "InsertLuItemPromotion");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "LuItemPromotion");
						GenerateActivityLog("LuItemPromotion", "Error");
						file.CopyTo(baseFolder + @"LuItemPromotion\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("LuItemPromotion", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"LuItemPromotion");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"LuItemPromotion\Archive\" + file.Name);

			}
			GenerateActivityLog("LuItemPromotion", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("LuTender")]

		public IHttpActionResult ConvertLuTender()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"LuTender");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							LuTenderModel luTenderModel = new LuTenderModel()
							{
								TenderCode = Convert.ToString(rec.tender_code),
								TenderDesc = Convert.ToString(rec.tender_desc),


							};
							
							modelInsertHelper.RunSPForModel(luTenderModel, "InsertLuTender");

						}

					}
					catch (Exception ex)
					{

						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "LuTender");
						GenerateActivityLog("LuTender", "Error");
						file.CopyTo(baseFolder + @"LuTender\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{
				GenerateActivityLog("LuTender", "Error");
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"LuTender");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(baseFolder + @"LuTender\Archive\" + file.Name);

			}
			GenerateActivityLog("LuTender", "Success");
			return Ok("Successful");
		}


		[HttpGet]
		[Route("SaleItem")]

		public IHttpActionResult ConvertSaleItem()
		{
			try
			{
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"SaleItem");
				FileInfo[] files = d.GetFiles("*.parquet");
				var reader = new ChoParquetReader();
				foreach (FileInfo file in files)
				{
					try
					{
						reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{
							SaleItemModel saleItemModel = new SaleItemModel()
							{
								TransNo = Convert.ToInt64(rec.trans_no),
								SaleItemNo = Convert.ToInt32(rec.sale_item_no),
								CompanyInd = Convert.ToInt32(rec.company_ind),
								Company = Convert.ToString(rec.company),
								Chain = Convert.ToString(rec.chain),
								DeptSubGroup = Convert.ToString(rec.dept_sub_group),
								ProductItemNo = Convert.ToString(rec.product_item_no),
								RealValue = Convert.ToInt32(rec.real_value),
								RealValueBc = Convert.ToInt32(rec.real_value_bc),
								SaleValue = Convert.ToInt32(rec.sale_value),
								SaleValueBc = Convert.ToInt32(rec.sale_value_bc),
								Quantity = Convert.ToInt32(rec.quantity),
								BranchCode = Convert.ToInt32(rec.branch_code),
								TransDate = Convert.ToString(rec.trans_date),
								TransTime = Convert.ToInt32(rec.trans_time),
								PriceLookupInd = Convert.ToString(rec.price_lookup_ind),
								SaleIndicator = Convert.ToInt32(rec.sale_indicator),
								ReturnIndicator = Convert.ToInt32(rec.return_indicator),
								ReturnReasonCode = Convert.ToString(rec.return_reason_code),
								VatRate = Convert.ToInt32(rec.vat_rate),
								FullUpos = Convert.ToString(rec.full_upos),
								ULabel = Convert.ToString(rec.u_label),
								SeqNo = Convert.ToInt32(rec.seq_no),
								ScannedKeyedInd = Convert.ToString(rec.scanned_keyed_ind),
								GiftReceipt = Convert.ToString(rec.gift_receipt),
								OrderIndicator = Convert.ToInt32(rec.order_indicator),
								PromoIndicator = Convert.ToInt32(rec.promo_indicator),
								CorrectIndicator = Convert.ToInt32(rec.correct_indicator),
								AllowIndicator = Convert.ToInt32(rec.allow_indicator),
								DiscountIndicator = Convert.ToInt32(rec.discount_indicator),
								NorIndicator = Convert.ToInt32(rec.nor_indicator),
								MultibuyIndicator = Convert.ToInt32(rec.multibuy_indicator),
								BranchOrig = Convert.ToString(rec.branch_orig),
								CustPostCode = Convert.ToString(rec.cust_post_code),
								UposMatchOverride = Convert.ToString(rec.upos_match_override),
								UposMatchOverrideBy = Convert.ToString(rec.upos_match_override_by),
								GriReceiptType = Convert.ToString(rec.gri_receipt_type),
								PartnerCode = Convert.ToString(rec.partner_code)
							};
							
							
							modelInsertHelper.RunSPForModel(saleItemModel, "InsertSaleItem");
							
						}
					}
					catch (Exception ex)
					{
						reader.Dispose();
						file.MoveTo(baseFolder + @"SaleItem\Errored\" + file.Name);
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "SaleItem");
						GenerateActivityLog("SaleItem", "Error");
						return BadRequest(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				
				GenerateActivityLog("SaleItem", "Error");
				return BadRequest("Errored with " + ex.Message);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"SaleItem");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"SaleItem\Archive\" + file.Name);
			}
			GenerateActivityLog("SaleItem", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("LuSundry")]

		public IHttpActionResult ConvertLuSundry()
		{
			try
			{
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"LuSundry");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{
						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{
							LuSundryModel luSundryModel = new LuSundryModel()
							{
								SundryCode = Convert.ToInt32(rec.sundry_code),
								SundryDesc = Convert.ToString(rec.sundry_desc),
								SundryType = Convert.ToString(rec.sundry_type),

							};
							
							modelInsertHelper.RunSPForModel(luSundryModel, "InsertLuSundry");
						}
					}
					catch (Exception ex)
					{
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "LuSundry");
						GenerateActivityLog("LuSundry", "Error");
						file.CopyTo(baseFolder + @"LuSundry\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				GenerateActivityLog("LuSundry", "Error");
				return BadRequest("Errored with " + ex.Message);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"LuSundry");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"LuSundry\Archive\" + file.Name);
			}
			GenerateActivityLog("LuSundry", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("LuVoid")]

		public IHttpActionResult ConvertLuVoid()
		{
			try
			{
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"LuVoid");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{
						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;
						
						while ((rec = reader.Read()) != null)
						{
							LuVoidModel luVoidModel = new LuVoidModel()
							{
								VoidCode = Convert.ToString(rec.void_code),
								VoidDesc = Convert.ToString(rec.void_desc),

							};
							
							modelInsertHelper.RunSPForModel(luVoidModel, "InsertLuVoid");
						}
					}
					catch (Exception ex)
					{
						
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "LuVoid");
						file.CopyTo(baseFolder + @"LuVoid\Errored\" + file.Name);
						GenerateActivityLog("LuVoid", "Error");
						return BadRequest(ex.Message);
						
					}
				}
			}
			catch (Exception ex)
			{
				GenerateActivityLog("LuVoid", "Error");
				return BadRequest("Errored with " + ex.Message);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"LuVoid");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"LuVoid\Archive\" + file.Name);
			}
			GenerateActivityLog("LuVoid", "Success");
			return Ok("Successful");
		}

		[HttpGet]
		[Route("SafeCountDetailsEvent")]

		public IHttpActionResult ConvertSafeCountDetailsEvent()
		{
			try
			{
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"SafeCountDetailsEvent");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{
						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{
							SafeCountDetailsEventModel safeCountDetailsEventModel = new SafeCountDetailsEventModel()
							{
								SafeCountDetailsEventKey = Convert.ToInt32(rec.safe_count_details_event_key),
								StoreNumber = Convert.ToInt32(rec.store_number),
								Date = Convert.ToString(rec.date),
								Time = Convert.ToString(rec.time),
								CountedValue = Convert.ToInt32(rec.counted_value),
								DiscrepancyAmount = Convert.ToInt32(rec.discrepancy_amount),
								CurrencyCode = Convert.ToString(rec.currency_code),
								PayrollNumber = Convert.ToInt64(rec.payroll_number)

							};
							
							modelInsertHelper.RunSPForModel(safeCountDetailsEventModel, "InsertSafeCountDetailsEvent");
						}
					}
					catch (Exception ex)
					{
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "SafeCountDetailsEvent");
						GenerateActivityLog("SafeCountDetailsEvent", "Error");
						file.CopyTo(baseFolder + @"SafeCountDetailsEvent\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				GenerateActivityLog("SafeCountDetailsEvent", "Error");
				return BadRequest("Errored with " + ex.Message);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"SafeCountDetailsEvent");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"SafeCountDetailsEvent\Archive\" + file.Name);
			}
			GenerateActivityLog("SafeCountDetailsEvent", "Success");
			return Ok("Successful");
		}


		[HttpGet]
		[Route("ConvertBankingCount")]

		public IHttpActionResult ConvertBankCount()
		{
			try
			{



				DirectoryInfo d = new DirectoryInfo(baseFolder + @"BankingCountDetailsEvent");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{


							BankingCountDetailsEventModel bankingCountDetailsEvent = new BankingCountDetailsEventModel()
							{
								BankingCountDetailsEventKey = Convert.ToInt32(rec.banking_count_details_event_key),
								StoreNumber = Convert.ToInt32(rec.store_number),
								Date = Convert.ToString(rec.date),
								Time = Convert.ToString(rec.time),
								CountType = Convert.ToInt32(rec.count_type),
								CountedValue = Convert.ToInt32(rec.counted_value),
								CurrencyCode = Convert.ToString(rec.currency_code),
								PayrollNumber = Convert.ToInt64(rec.payroll_number)



							};

							modelInsertHelper.RunSPForModel(bankingCountDetailsEvent, "InsertBankingCountDetailsEvent");

						}

					}
										
					catch (Exception ex)
								{
									
									modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "BankingCountDetailsEvent");
									GenerateActivityLog("BankingCountDetailsEvent", "Error");
									file.CopyTo(baseFolder + @"BankingCountDetailsEvent\Errored\" + file.Name);
									return BadRequest(ex.Message);
								}
							}
						}
								catch (Exception ex)
								{
									GenerateActivityLog("BankingCountDetailsEvent", "Error");
									return BadRequest("Errored with " + ex.Message);
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"BankingCountDetailsEvent");
					FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
					foreach (FileInfo file in files2)
					{
						file.MoveTo(baseFolder + @"BankingCountDetailsEvent\Archive\" + file.Name);
					}
					GenerateActivityLog("BankingCountDetailsEvent", "Success");
					return Ok("Successful");
							}

		












		[HttpGet]
		[Route("SundryBranch")]

		public IHttpActionResult ConvertSundryBranch()
		{
			try
			{
				DirectoryInfo d = new DirectoryInfo(baseFolder + @"SundryBranch");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{
						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null)
						{
							SundryBranchModel sundryBranchModel = new SundryBranchModel()
							{
								BranchCode = Convert.ToInt32(rec.branch_code),
								TransNo = Convert.ToInt64(rec.trans_no),
								SundryCode = Convert.ToInt32(rec.sundry_code),
								SundryValue = Convert.ToInt32(rec.sundry_value),
								SundryValueBc = Convert.ToInt32(rec.sundry_value_bc),
								SundryReference = Convert.ToString(rec.sundry_reference),
								SundryVoid = Convert.ToString(rec.sundry_void),
								SundryQuantity = Convert.ToInt32(rec.sundry_quantity)

							};
							
							modelInsertHelper.RunSPForModel(sundryBranchModel, "InsertSundryBranch");
						}
					}
					catch (Exception ex)
					{
						
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "SundryBranch");
						GenerateActivityLog("SundryBranch", "Error");
						file.MoveTo(baseFolder + @"SundryBranch\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				GenerateActivityLog("SundryBranch", "Error");
				return BadRequest("Errored with " + ex.Message);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(baseFolder + @"SundryBranch");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(baseFolder + @"SundryBranch\Archive\" + file.Name);
			}
			GenerateActivityLog("SundryBranch", "Success");
			return Ok("Successful");
		}



		private void GenerateActivityLog(string routename, string result)
        {
			ApiLogModel apiLog = new ApiLogModel()
			{
				ApiName = routename,
				LastActivity = DateTime.Now,
				LastResult = result
			    
			};

			
			modelInsertHelper.RunSPForModel(apiLog, "UpdateActivityLog");
        }



	}


}
