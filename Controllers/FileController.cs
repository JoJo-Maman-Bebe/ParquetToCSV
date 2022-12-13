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
		public bool stopReader = false;

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


		
				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\Warehouse740Transactions");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach(FileInfo file in files)
				{
					try
					{


					
					var reader = new ChoParquetReader(file.FullName);
					dynamic rec;

					while ((rec = reader.Read()) != null || stopReader == true)
					{
						if(stopReader == true)
							{
								reader.Dispose();
							}

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
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.Insert740Transaction(warehouse740TransactionModel);
					}
					
					}
					catch(Exception ex)
					{
						stopReader = true;
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "740 Transactions");
						
						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\Warehouse740Transactions\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				
				return BadRequest("Errored with " + ex.Message);

			}

			
			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\Warehouse740Transactions");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\Warehouse740Transactions\Archive\" + file.Name);

			}

			return Ok("Successful");
				
			}





		[HttpGet]
		[Route("BankingOverShort")]

		public IHttpActionResult ConvertBankingOverShort()
		{



			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\BankingOverShort");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
						{
							if (stopReader == true)
							{
								reader.Dispose();
							}

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
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertBankingOverShort(bankingOverShortModel);
						}

					}
					catch (Exception ex)
					{
						stopReader = true;
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Banking Over Short");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\BankingOverShort\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{
				stopReader = true;
				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\BankingOverShort");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\BankingOverShort\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		[HttpGet]
		[Route("ItemPromotion")]

		public IHttpActionResult ConvertItemPromotion()
		{



			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
						{
							if (stopReader == true)
							{
								reader.Dispose();
							}

							ItemPromotionModel itemPromotionModel = new ItemPromotionModel()
							{
								SaleItemNo = Convert.ToInt32(rec.sale_item_no),
								TransNo = Convert.ToInt32(rec.trans_no),
								PromotionCode = Convert.ToInt32(rec.promotion_code),
								DiscountValue = Convert.ToInt32(rec.discount_value),
								DiscountValueBc = Convert.ToInt32(rec.discount_value_bc),
								DiscountRate = Convert.ToInt32(rec.discount_rate),
							};
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertItemPromotion(itemPromotionModel);
						}

					}
					catch (Exception ex)
					{
						stopReader = true;
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Item Promotion");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion\Archive\" + file.Name);

			}

			return Ok("Successful");

		}


		
		
		[HttpGet]
		[Route("StkBalDaily")]

		public IHttpActionResult ConvertStkBranchBalDaily()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StkBranchBalDaily");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
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
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertStkBalDaily(stkBranchBalDailyModel);
							
						}

					}
					catch (Exception ex)
					{
						
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Stk Branch Bal Daily");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\StkBranchBalDaily\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StkBranchBalDaily");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				
				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\StkBranchBalDaily\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		[HttpGet]
		[Route("TillCountDetailsEvent")]

		public IHttpActionResult ConvertTillCountDetEvent()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\TillCountDetailsEvent");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
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
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertTillCountDetEvent(tillCountDetailsEventModel);

						}

					}
					catch (Exception ex)
					{

						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Till Count Details Event");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\TillCountDetailsEvent\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\TillCountDetailsEvent");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\TillCountDetailsEvent\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		[HttpGet]
		[Route("Stocktake")]

		public IHttpActionResult ConvertStockTake()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StockTake");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
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
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertStocktake(stocktakeModel);

						}

					}
					catch (Exception ex)
					{

						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Stock Take");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\StockTake\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StockTake");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\StockTake\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		[HttpGet]
		[Route("StkMovements")]

		public IHttpActionResult ConvertStkMovements()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StkMovements");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
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
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertStkMovement(stkMovementModel);

						}

					}
					catch (Exception ex)
					{

						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "StkMovements");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\StkMovements\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\StkMovements");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\StkMovements\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		

		[HttpGet]
		[Route("DgBranch")]

		public IHttpActionResult ConvertDgBranch()
		{


			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\DgBranch");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
						{


							DgBranchModel dgBranchModel = new DgBranchModel()
							{
								BranchCode = Convert.ToInt32(rec.branch_code),
								BranchDesc = Convert.ToString(rec.branch_desc),
								AreaCode = Convert.ToInt32(rec.area_code),
								BranchAddress1 = Convert.ToString(rec.branch_address_1),
								BranchAddress2 = Convert.ToString(rec.branch_address_2),
								BranchAddress3 = Convert.ToString(rec.branch_address_3),
								BranchAddress4 = Convert.ToString(rec.branch_address_4),
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
								AlClosingFri = Convert.ToString(rec.al_closing_fri),
								AlClosingSat = Convert.ToString(rec.al_closing_sat),
								AlClosingSun = Convert.ToString(rec.al_closing_sun),
								BranchType = Convert.ToString(rec.branch_type),
								CompanyInd = Convert.ToInt32(rec.company_ind),
								PartnerCode = Convert.ToString(rec.partner_code)
							};
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.RunSPForModel(dgBranchModel, "InsertDgBranch");

						}

					}
					catch (Exception ex)
					{

						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "DgBranch");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\DgBranch\Errored\" + file.Name);
						return BadRequest(ex.Message);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\DgBranch");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{

				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\DgBranch\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

		[HttpGet]
		[Route("Template")]

		public IHttpActionResult TemplateMethod()
		{



			try
			{



				DirectoryInfo d = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion");
				FileInfo[] files = d.GetFiles("*.parquet");

				foreach (FileInfo file in files)
				{
					try
					{



						var reader = new ChoParquetReader(file.FullName);
						dynamic rec;

						while ((rec = reader.Read()) != null || stopReader == true)
						{
							if (stopReader == true)
							{
								reader.Dispose();
							}

							ItemPromotionModel itemPromotionModel = new ItemPromotionModel()
							{
								SaleItemNo = Convert.ToInt32(rec.sale_item_no),
								TransNo = Convert.ToInt32(rec.trans_no),
								PromotionCode = Convert.ToInt32(rec.promotion_code),
								DiscountValue = Convert.ToInt32(rec.discount_value),
								DiscountValueBc = Convert.ToInt32(rec.discount_value_bc),
								DiscountRate = Convert.ToInt32(rec.discount_rate),
							};
							ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
							modelInsertHelper.InsertItemPromotion(itemPromotionModel);
						}

					}
					catch (Exception ex)
					{
						stopReader = true;
						ModelInsertHelper modelInsertHelper = new ModelInsertHelper();
						modelInsertHelper.InsertErrorLog(ex.Message, ex.StackTrace, file.FullName, "Item Promotion");

						file.CopyTo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion\Errored\" + file.Name);
					}
				}

			}
			catch (Exception ex)
			{

				return BadRequest("Errored with " + ex.Message);

			}


			DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion");
			FileInfo[] files2 = directoryInfo.GetFiles("*.parquet");
			foreach (FileInfo file in files2)
			{
				file.MoveTo(@"C:\JoJo Maman Bébé\NEXT\ItemPromotion\Archive\" + file.Name);

			}

			return Ok("Successful");

		}

	}


}
