using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace ConnectionManagerDb
{
    public class TuincentrumManager
    {
        public int KortingOpPlantPrijzen(decimal getal)
        {
            var dbManager = new TuincentrumDbManager();
            using (var conTuincentrum = dbManager.GetConnection())
            {
                using (var comKorting = conTuincentrum.CreateCommand())
                {
                    comKorting.CommandType = CommandType.StoredProcedure;
                    comKorting.CommandText = "AddKorting";

                    DbParameter parPrijs = comKorting.CreateParameter();
                    parPrijs.ParameterName = "@Korting";
                    parPrijs.Value = getal;
                    parPrijs.DbType = DbType.Decimal;
                    comKorting.Parameters.Add(parPrijs);

                    conTuincentrum.Open();
                    return comKorting.ExecuteNonQuery();
                }
            }
        }

        public Boolean InsertLeverancier(string naam, string adres, string postNr, string woonplaats)
        {
            var dbManager = new TuincentrumDbManager();
            using (var conTuincentrum = dbManager.GetConnection())
            {
                using (var comInsertLeverancier = conTuincentrum.CreateCommand())
                {
                    comInsertLeverancier.CommandType = CommandType.StoredProcedure;
                    comInsertLeverancier.CommandText = "AddLeverancier";

                    IDbDataParameter parNaam = comInsertLeverancier.CreateParameter();
                    parNaam.ParameterName = "@Naam";
                    parNaam.Value = naam;
                    comInsertLeverancier.Parameters.Add(parNaam);

                    IDbDataParameter parAdres = comInsertLeverancier.CreateParameter();
                    parAdres.ParameterName = "@Adres";
                    parAdres.Value = adres;
                    comInsertLeverancier.Parameters.Add(parAdres);

                    IDbDataParameter parPostNr = comInsertLeverancier.CreateParameter();
                    parPostNr.ParameterName = "@PostNr";
                    parPostNr.Value = postNr;
                    comInsertLeverancier.Parameters.Add(parPostNr);

                    IDbDataParameter parWoonplaats = comInsertLeverancier.CreateParameter();
                    parWoonplaats.ParameterName = "@Woonplaats";
                    parWoonplaats.Value = woonplaats;
                    comInsertLeverancier.Parameters.Add(parWoonplaats);

                    conTuincentrum.Open();
                    return comInsertLeverancier.ExecuteNonQuery() != 0;
                }
            }
        }

        public void VervangLeverancier(int oudLeverancier, int nieuwLeverancier)
        {
            var dbManager = new TuincentrumDbManager();
            var opties = new TransactionOptions();
            opties.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

            using (var traVervangLeverancier = new TransactionScope(TransactionScopeOption.Required, opties))
            {
                using (var conTuin = dbManager.GetConnection())
                {
                    using (var comVervangen = conTuin.CreateCommand())
                    {
                        comVervangen.CommandType = CommandType.StoredProcedure;
                        comVervangen.CommandText = "VervangLeverancier";

                        var parOudLeverancier = comVervangen.CreateParameter();
                        parOudLeverancier.ParameterName = "@oudLeverancier";
                        parOudLeverancier.Value = oudLeverancier;
                        comVervangen.Parameters.Add(parOudLeverancier);

                        var parNieuwLeverancier = comVervangen.CreateParameter();
                        parNieuwLeverancier.ParameterName = "@nieuwLeverancier";
                        parNieuwLeverancier.Value = nieuwLeverancier;
                        comVervangen.Parameters.Add(parNieuwLeverancier);

                        conTuin.Open();
                        if (comVervangen.ExecuteNonQuery() == 0)
                            throw new Exception("Leverancier bestaat niet");
                    }

                    using (var comVerwijderen = conTuin.CreateCommand())
                    {
                        comVerwijderen.CommandType = CommandType.StoredProcedure;
                        comVerwijderen.CommandText = "VerwijderLeverancier";

                        var parOudLeverancier = comVerwijderen.CreateParameter();
                        parOudLeverancier.ParameterName = "@oudLeverancier";
                        parOudLeverancier.Value = oudLeverancier;
                        comVerwijderen.Parameters.Add(parOudLeverancier);

                        if (comVerwijderen.ExecuteNonQuery() == 0)
                            throw new Exception("Oud leverancier bestaat niet");
                    }
                    traVervangLeverancier.Complete();
                }
            }
        }

        public decimal BerekenGemiddeldePrijs(string soort)
        {
            var dbManager = new TuincentrumDbManager();
            using (var conTuin = dbManager.GetConnection())
            {
                using (var comGemiddelde = conTuin.CreateCommand())
                {
                    comGemiddelde.CommandType = CommandType.StoredProcedure;
                    comGemiddelde.CommandText = "GemiddeldePrijs";

                    DbParameter parSoort = comGemiddelde.CreateParameter();
                    parSoort.ParameterName = "@Soort";
                    parSoort.Value = soort;
                    comGemiddelde.Parameters.Add(parSoort);

                    conTuin.Open();
                    object resultaat = comGemiddelde.ExecuteScalar();
                    if (resultaat == null)
                        throw new Exception("Soort niet gevonden");
                    else
                        return (decimal)resultaat;
                }
            }
        }
    }
}
