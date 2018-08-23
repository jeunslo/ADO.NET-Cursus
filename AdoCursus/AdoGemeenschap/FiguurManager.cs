using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoGemeenschap
{
    public class FiguurManager
    {
        public List<Figuur> GetFiguren()
        {
            List<Figuur> figuren = new List<Figuur>();
            var manager = new StripManager();
            using (var conStrip = manager.GetConnection())
            {
                using (var comFiguren = conStrip.CreateCommand())
                {
                    comFiguren.CommandType = CommandType.Text;
                    comFiguren.CommandText = "select * from Figuren";

                    conStrip.Open();
                    using (var rdrFiguren = comFiguren.ExecuteReader())
                    {
                        Int32 IDPos = rdrFiguren.GetOrdinal("ID");
                        Int32 NaamPos = rdrFiguren.GetOrdinal("Naam");
                        Int32 VersiePos = rdrFiguren.GetOrdinal("Versie");

                        while(rdrFiguren.Read())
                        {
                            figuren.Add(new Figuur(rdrFiguren.GetInt32(IDPos), rdrFiguren.GetString(NaamPos), rdrFiguren.GetValue(VersiePos)));
                        }
                    }
                }
            }
            return figuren;
        }

        public void SchrijfWijzigingen(List<Figuur> figuren)
        {
            var manager = new StripManager();
            using (var conStrip = manager.GetConnection())
            {
                using (var comUpdate = conStrip.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update Figuren set Naam = @naam where ID = @id and Versie = @versie";

                    var parNaam = comUpdate.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comUpdate.Parameters.Add(parNaam);

                    var parID = comUpdate.CreateParameter();
                    parID.ParameterName = "@id";
                    comUpdate.Parameters.Add(parID);

                    var parVersie = comUpdate.CreateParameter();
                    parVersie.ParameterName = "@versie";
                    comUpdate.Parameters.Add(parVersie);

                    conStrip.Open();
                    foreach (var eenFiguur in figuren)
                    {
                        parNaam.Value = eenFiguur.Naam;
                        parID.Value = eenFiguur.ID;
                        parVersie.Value = eenFiguur.Versie;
                        if(comUpdate.ExecuteNonQuery()==0)
                            throw new Exception("Iemand was je voor");
                    }
                }
            }
        }
    }
}
