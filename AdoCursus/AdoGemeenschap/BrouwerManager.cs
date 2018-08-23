using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace AdoGemeenschap
{
    public class BrouwerManager
    {
        public ObservableCollection<Brouwer> GetBrouwersBeginNaam(String beginNaam)
        {
            ObservableCollection<Brouwer> brouwers = new ObservableCollection<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comBrouwers = conBieren.CreateCommand())
                {
                    comBrouwers.CommandType = CommandType.Text;
                    if (beginNaam != string.Empty)
                    {
                        comBrouwers.CommandText = "select * from Brouwers where BrNaam like @zoals order by BrNaam";
                        var parZoals = comBrouwers.CreateParameter();
                        parZoals.ParameterName = "@zoals";
                        parZoals.Value = beginNaam + "%";
                        comBrouwers.Parameters.Add(parZoals);
                    }
                    else
                        comBrouwers.CommandText = "select * from Brouwers";

                    conBieren.Open();
                    using (var rdrBrouwers = comBrouwers.ExecuteReader())
                    {
                        Int32 brouwerNrPos = rdrBrouwers.GetOrdinal("BrouwerNr");
                        Int32 brNaamPos = rdrBrouwers.GetOrdinal("BrNaam");
                        Int32 adresPos = rdrBrouwers.GetOrdinal("Adres");
                        Int32 postcodePos = rdrBrouwers.GetOrdinal("Postcode");
                        Int32 gemeentePos = rdrBrouwers.GetOrdinal("Gemeente");
                        Int32 omzetPos = rdrBrouwers.GetOrdinal("omzet");

                        Int32? omzet;
                        while(rdrBrouwers.Read())
                        {
                            if (rdrBrouwers.IsDBNull(omzetPos))
                                omzet = null;
                            else
                                omzet = rdrBrouwers.GetInt32(omzetPos);
                            brouwers.Add(new Brouwer(rdrBrouwers.GetInt32(brouwerNrPos), rdrBrouwers.GetString(brNaamPos),
                                                     rdrBrouwers.GetString(adresPos), rdrBrouwers.GetInt16(postcodePos),
                                                     rdrBrouwers.GetString(gemeentePos), omzet));
                        }
                    }
                    
                }
            }
            return brouwers;
        }

        public List<Brouwer> SchrijfVerwijderingen(List<Brouwer> brouwers)
        {
            List<Brouwer> nietVerwijderdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                conBieren.Open();
                using (var comDelete = conBieren.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "delete from brouwers where BrouwerNr = @brouwernr";

                    var parBrouwerNr = comDelete.CreateParameter();
                    parBrouwerNr.ParameterName = "@brouwernr";
                    comDelete.Parameters.Add(parBrouwerNr);

                    foreach (Brouwer eenBrouwer in brouwers)
                    {
                        try
                        {
                            parBrouwerNr.Value = eenBrouwer.BrouwerNr;
                            if (comDelete.ExecuteNonQuery() == 0)
                                nietVerwijderdeBrouwers.Add(eenBrouwer);
                        }
                        catch(Exception)
                        {
                            nietVerwijderdeBrouwers.Add(eenBrouwer);
                        }
                    }
                }
            }
            return nietVerwijderdeBrouwers;
        }

        public List<Brouwer> SchrijfToevoegingen(List<Brouwer> brouwers)
        {
            List<Brouwer> nietToegevoegdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comInsert = conBieren.CreateCommand())
                {
                    comInsert.CommandType = CommandType.Text;
                    comInsert.CommandText = "Insert into brouwers (BrNaam, Adres, Postcode, Gemeente, Omzet) values (@brnaam, @adres, @postcode, @gemeente, @omzet)";

                    var parBrNaam = comInsert.CreateParameter();
                    parBrNaam.ParameterName = "@brnaam";
                    comInsert.Parameters.Add(parBrNaam);

                    var parAdres = comInsert.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comInsert.Parameters.Add(parAdres);

                    var parPostcode = comInsert.CreateParameter();
                    parPostcode.ParameterName = "@postcode";
                    comInsert.Parameters.Add(parPostcode);

                    var parGemeente = comInsert.CreateParameter();
                    parGemeente.ParameterName = "@gemeente";
                    comInsert.Parameters.Add(parGemeente);

                    var parOmzet = comInsert.CreateParameter();
                    parOmzet.ParameterName = "@omzet";
                    comInsert.Parameters.Add(parOmzet);

                    conBieren.Open();

                    foreach(Brouwer eenBrouwer in brouwers)
                    {
                        try
                        {
                            parBrNaam.Value = eenBrouwer.Naam;
                            parGemeente.Value = eenBrouwer.Gemeente;
                            parPostcode.Value = eenBrouwer.Postcode;
                            if (eenBrouwer.Omzet.HasValue)
                                parOmzet.Value = eenBrouwer.Omzet;
                            else
                                parOmzet.Value = DBNull.Value;
                            parAdres.Value = eenBrouwer.Adres;
                            if (comInsert.ExecuteNonQuery() == 0)
                                nietToegevoegdeBrouwers.Add(eenBrouwer);
                        }
                        catch(Exception)
                        {
                            nietToegevoegdeBrouwers.Add(eenBrouwer);
                        }
                    }

                }
            }
            return nietToegevoegdeBrouwers;
        }

        public List<Brouwer> SchrijfWijzigingen(List<Brouwer> brouwers)
        {
            List<Brouwer> nietDoorgevoerdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comUpdate = conBieren.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update brouwers set BrNaam = @naam, Adres = @adres, Postcode = @postcode, Gemeente = @gemeente, Omzet = @omzet where BrouwerNr = @brouwernr";

                    var parNaam = comUpdate.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comUpdate.Parameters.Add(parNaam);

                    var parAdres = comUpdate.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comUpdate.Parameters.Add(parAdres);

                    var parPostcode = comUpdate.CreateParameter();
                    parPostcode.ParameterName = "@postcode";
                    comUpdate.Parameters.Add(parPostcode);

                    var parGemeente = comUpdate.CreateParameter();
                    parGemeente.ParameterName = "@gemeente";
                    comUpdate.Parameters.Add(parGemeente);

                    var parOmzet = comUpdate.CreateParameter();
                    parOmzet.ParameterName = "@omzet";
                    comUpdate.Parameters.Add(parOmzet);

                    var parBrouwerNr = comUpdate.CreateParameter();
                    parBrouwerNr.ParameterName = "@brouwernr";
                    comUpdate.Parameters.Add(parBrouwerNr);

                    conBieren.Open();
                    foreach (Brouwer eenBrouwer in brouwers)
                    {
                        try
                        {
                            parNaam.Value = eenBrouwer.Naam;
                            parAdres.Value = eenBrouwer.Adres;
                            parGemeente.Value = eenBrouwer.Gemeente;
                            parPostcode.Value = eenBrouwer.Postcode;
                            parBrouwerNr.Value = eenBrouwer.BrouwerNr;
                            if (eenBrouwer.Omzet.HasValue)
                                parOmzet.Value = eenBrouwer.Omzet;
                            else
                                parOmzet.Value = DBNull.Value;
                            if (comUpdate.ExecuteNonQuery() == 0)
                                nietDoorgevoerdeBrouwers.Add(eenBrouwer);
                        }
                        catch(Exception)
                        {
                            nietDoorgevoerdeBrouwers.Add(eenBrouwer);
                        }
                    }
                }
            }
            return nietDoorgevoerdeBrouwers;
        }
    }
}
