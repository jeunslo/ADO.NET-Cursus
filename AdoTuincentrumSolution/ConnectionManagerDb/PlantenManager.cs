using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace ConnectionManagerDb
{
    public class PlantenManager
    {
        public ObservableCollection<Plant> GetPlantenList()
        {
            ObservableCollection<Plant> planten = new ObservableCollection<Plant>();
            var manager = new TuincentrumDbManager();
            using (var conTuin = manager.GetConnection())
            {
                using (var comPlantenList = conTuin.CreateCommand())
                {
                    comPlantenList.CommandType = CommandType.StoredProcedure;
                    comPlantenList.CommandText = "GetPlantenTable";

                    conTuin.Open();
                    using (var rdrPlanten = comPlantenList.ExecuteReader())
                    {
                        Int32 plantNrPos = rdrPlanten.GetOrdinal("PlantNr");
                        Int32 plantNaamPos = rdrPlanten.GetOrdinal("Naam");
                        Int32 soortPos = rdrPlanten.GetOrdinal("Soort");
                        Int32 levNrPos = rdrPlanten.GetOrdinal("LevNr");
                        Int32 kleurPos = rdrPlanten.GetOrdinal("Kleur");
                        Int32 prijsPos = rdrPlanten.GetOrdinal("VerkoopPrijs");

                        while(rdrPlanten.Read())
                        {
                            planten.Add(new Plant(rdrPlanten.GetInt32(plantNrPos), rdrPlanten.GetString(plantNaamPos),
                                                  rdrPlanten.GetString(soortPos), rdrPlanten.GetInt32(levNrPos),
                                                  rdrPlanten.GetString(kleurPos), rdrPlanten.GetDecimal(prijsPos)));
                        }
                    }
                }
            }
            return planten;
        }

        public List<Plant> SchrijfGewijzigdePlanten(List<Plant> planten)
        {
            List<Plant> nietGewijzigdePlanten = new List<Plant>();
            var manager = new TuincentrumDbManager();
            using (var conTuin = manager.GetConnection())
            {
                using (var comUpdate = conTuin.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update Planten set Kleur = @kleur, VerkoopPrijs = @verkoopprijs where PlantNr=@plantnr";

                    var parKleur = comUpdate.CreateParameter();
                    parKleur.ParameterName = "@kleur";
                    comUpdate.Parameters.Add(parKleur);

                    var parPrijs = comUpdate.CreateParameter();
                    parPrijs.ParameterName = "@verkoopprijs";
                    comUpdate.Parameters.Add(parPrijs);

                    var parPlantNr = comUpdate.CreateParameter();
                    parPlantNr.ParameterName = "@plantnr";
                    comUpdate.Parameters.Add(parPlantNr);

                    conTuin.Open();
                    foreach (Plant p in planten)
                    {
                        try
                        {
                            parKleur.Value = p.Kleur;
                            parPlantNr.Value = p.PlantNr;
                            parPrijs.Value = p.VerkoopPrijs;

                            if (comUpdate.ExecuteNonQuery() == 0)
                                nietGewijzigdePlanten.Add(p);
                        }
                        catch (Exception)
                        {
                            nietGewijzigdePlanten.Add(p);
                        }
                    }
                }
            }
            return nietGewijzigdePlanten;
        }
    }
}
