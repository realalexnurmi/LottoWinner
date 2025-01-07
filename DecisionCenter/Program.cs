using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using LottoClassLibrary;

namespace DecisionCenter
{
	public class Program
	{
		static void Main(string[] args)
		{
			string connectionString = "server=localhost; database=Nurmi; integrated security=sspi; packet size=4096; connection lifetime=500; min pool size=1; max pool size=50; TrustServerCertificate=true";
			List<LottoTicket> tickets = new List<LottoTicket>();
			bool doing = true;

			while (tickets.Count < 33)
			{
				LottoSearchData searchData = GetSearchData(connectionString);
				if (searchData.IsValid())
				{
					LottoTicket ticket = searchData.GenerateRandomTicket();
					ticket.PrintTicket();
					if (InsertTicket(ticket, connectionString))
					{
						tickets.Add(ticket);
					}
				}
			}
		}

		static LottoSearchData GetSearchData(string connectionString)
		{
			LottoSearchData searchData = new LottoSearchData();
			// Подключение к базе данных
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				// Вызов хранимой процедуры для получения настроек поиска
				using (SqlCommand command = new SqlCommand("usp_GetSearchSettings", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (SqlDataReader reader = command.ExecuteReader())
					{
						// Чтение данных из первого датасета
						while (reader.Read())
						{
							searchData.SearchedFieldTypes.Add(new LottoFieldType(reader.GetInt32(0), reader.GetInt32(1)));
						}

						// Чтение данных из второго датасета
						if (reader.NextResult())
						{
							while (reader.Read())
							{
								searchData.SearchedColumns.Add(new LottoColumn(reader.GetInt32(0), new LottoNumber(reader.GetInt32(1)), !reader.IsDBNull(2) ? new LottoNumber(reader.GetInt32(2)) : null));
							}
						}
					}
				}
			}
			return searchData;
		}

		// Метод для вставки билета в БД
		static bool InsertTicket(LottoTicket ticket, string connectionString)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				// Вставка билета
				using (SqlCommand command = new SqlCommand("usp_CreateNewTicket", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@TicketNumber", ticket.TicketNumber));
					command.Parameters.Add(new SqlParameter("@FCWO_F1", ticket.FirstField.FieldType.FirstColumnWithOneNumber));
					command.Parameters.Add(new SqlParameter("@SCWO_F1", ticket.FirstField.FieldType.SecondColumnWithOneNumber));
					command.Parameters.Add(new SqlParameter("@RFD_F1", ticket.FirstField.ToDataTable()));
					command.Parameters.Add(new SqlParameter("@FCWO_F2", ticket.SecondField.FieldType.FirstColumnWithOneNumber));
					command.Parameters.Add(new SqlParameter("@SCWO_F2", ticket.SecondField.FieldType.SecondColumnWithOneNumber));
					command.Parameters.Add(new SqlParameter("@RFD_F2", ticket.SecondField.ToDataTable()));
					command.Parameters.Add(new SqlParameter("@GuidCreatedTicket", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });
					command.ExecuteNonQuery();
				}
			}

			return true;
		}
	}
}
