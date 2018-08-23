using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class VideotheekManager
    {
        public ObservableCollection<Film> GetFilmsList()
        {
            ObservableCollection<Film> filmsList = new ObservableCollection<Film>();
            var manager = new VideotheekDbManager();

            using (var conVideo = manager.GetConnection())
            {
                using (var comGetList = conVideo.CreateCommand())
                {
                    comGetList.CommandType = CommandType.Text;
                    comGetList.CommandText = "Select Films.BandNr as BandNr, Films.Titel as Titel, Genres.Genre as Genre, Films.InVoorraad as InVoorraad, Films.UitVoorraad as UitVoorraad, Films.Prijs as Prijs, Films.TotaalVerhuurd as TotaalVerhuurd FROM Films INNER JOIN Genres On Films.GenreNr = Genres.GenreNr order by Titel";

                    conVideo.Open();
                    using (var rdrFilms = comGetList.ExecuteReader())
                    {
                        Int32 bandNrPos = rdrFilms.GetOrdinal("BandNr");
                        Int32 titelPos = rdrFilms.GetOrdinal("Titel");
                        Int32 genrePos = rdrFilms.GetOrdinal("Genre");
                        Int32 inVoorraadPos = rdrFilms.GetOrdinal("InVoorraad");
                        Int32 uitVoorraadPos = rdrFilms.GetOrdinal("UitVoorraad");
                        Int32 prijsPos = rdrFilms.GetOrdinal("Prijs");
                        Int32 totaalVerhuurdPos = rdrFilms.GetOrdinal("TotaalVerhuurd");

                        while(rdrFilms.Read())
                        {
                            filmsList.Add(new Film(rdrFilms.GetInt32(bandNrPos), rdrFilms.GetString(titelPos),
                                                    rdrFilms.GetString(genrePos), rdrFilms.GetInt32(inVoorraadPos),
                                                    rdrFilms.GetInt32(uitVoorraadPos), rdrFilms.GetDecimal(prijsPos),
                                                    rdrFilms.GetInt32(totaalVerhuurdPos)));
                        }
                    }
                }
            }
            return filmsList;
        }

        public List<String> getGenreList()
        {
            List<String> genreList = new List<String>();
            var manager = new VideotheekDbManager();

            using (var conVideo = manager.GetConnection())
            {
                using (var comGetList = conVideo.CreateCommand())
                {
                    comGetList.CommandType = CommandType.Text;
                    comGetList.CommandText = "Select Genres.Genre as Genre from Genres order by Genres.Genre";

                    conVideo.Open();
                    using (var rdrGenres = comGetList.ExecuteReader())
                    {
                        Int32 genreNaamPos = rdrGenres.GetOrdinal("Genre");
                        while (rdrGenres.Read())
                        {
                            genreList.Add(rdrGenres.GetString(genreNaamPos));
                        }
                    }
                }
            }
            return genreList;
        }
        

        public List<Film> SchrijfToevoegingen(List<Film> filmsList)
        {
            List<Film> nietToegevoegdeFilms = new List<Film>();
            var manager = new VideotheekDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comInsert = conVideo.CreateCommand())
                {
                    comInsert.CommandType = CommandType.Text;
                    comInsert.CommandText = "Insert into Films (Titel, GenreNr, InVoorraad, UitVoorraad, Prijs, TotaalVerhuurd) values (@titel, (Select Genres.GenreNr From Genres where Genres.Genre = @genre), @inVoorraad, @uitVoorraad, @prijs, @totaalVerhuurd)";

                    var parTitel = comInsert.CreateParameter();
                    parTitel.ParameterName = "@titel";
                    comInsert.Parameters.Add(parTitel);

                    var parGenre = comInsert.CreateParameter();
                    parGenre.ParameterName = "@genre";
                    comInsert.Parameters.Add(parGenre);

                    var parInVoorraad = comInsert.CreateParameter();
                    parInVoorraad.ParameterName = "@inVoorraad";
                    comInsert.Parameters.Add(parInVoorraad);

                    var parUitVoorraad = comInsert.CreateParameter();
                    parUitVoorraad.ParameterName = "@uitVoorraad";
                    comInsert.Parameters.Add(parUitVoorraad);

                    var parPrijs = comInsert.CreateParameter();
                    parPrijs.ParameterName = "@prijs";
                    comInsert.Parameters.Add(parPrijs);

                    var parTotaalVerhuurd = comInsert.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@totaalVerhuurd";
                    comInsert.Parameters.Add(parTotaalVerhuurd);

                    conVideo.Open();

                    foreach (Film eenFilm in filmsList)
                    {
                        try
                        {
                            parTitel.Value = eenFilm.Titel;
                            parInVoorraad.Value = eenFilm.InVoorraad;
                            parUitVoorraad.Value = eenFilm.UitVoorraad;
                            parPrijs.Value = eenFilm.Prijs;
                            parTotaalVerhuurd.Value = eenFilm.TotaalVerhuurd;
                            parGenre.Value = eenFilm.Genre;
                            
                            if (comInsert.ExecuteNonQuery() == 0)
                                nietToegevoegdeFilms.Add(eenFilm);
                        }
                        catch (Exception ex)
                        {
                            nietToegevoegdeFilms.Add(eenFilm);
                        }
                    }
                }
            }
            return nietToegevoegdeFilms;
        }

        public List<Film> SchrijfVerwijderingen(List<Film> filmsList)
        {
            List<Film> nietVerwijderdeFilms = new List<Film>();
            var manager = new VideotheekDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comDelete = conVideo.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "delete from Films where BandNr = @bandNr";

                    var parBandNr = comDelete.CreateParameter();
                    parBandNr.ParameterName = "@bandNr";
                    comDelete.Parameters.Add(parBandNr);

                    conVideo.Open();
                    foreach (Film eenFilm in filmsList)
                    {
                        try
                        {
                            parBandNr.Value = eenFilm.BandNr;
                            if (comDelete.ExecuteNonQuery() == 0)
                                nietVerwijderdeFilms.Add(eenFilm);
                        }
                        catch(Exception ex)
                        {
                            nietVerwijderdeFilms.Add(eenFilm);
                        }
                    }
                }
            }
            return nietVerwijderdeFilms;
        }

        public List<Film> SchrijfWijzigingen(List<Film> filmsList)
        {
            List<Film> nietGewijzigdeFilms = new List<Film>();
            var manager = new VideotheekDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comUpdate = conVideo.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "Update Films set InVoorraad = @inVoorraad, UitVoorraad = @uitVoorraad, TotaalVerhuurd = @totaalVerhuurd where BandNr = @BandNr";

                    var parBandNr = comUpdate.CreateParameter();
                    parBandNr.ParameterName = "@bandNr";
                    comUpdate.Parameters.Add(parBandNr);

                    var parInVoorraad = comUpdate.CreateParameter();
                    parInVoorraad.ParameterName = "@inVoorraad";
                    comUpdate.Parameters.Add(parInVoorraad);

                    var parUitVoorraad = comUpdate.CreateParameter();
                    parUitVoorraad.ParameterName = "@uitVoorraad";
                    comUpdate.Parameters.Add(parUitVoorraad);

                    var parTotaalVerhuurd = comUpdate.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@totaalVerhuurd";
                    comUpdate.Parameters.Add(parTotaalVerhuurd);

                    conVideo.Open();

                    foreach (Film eenFilm in filmsList)
                    {
                        try
                        {
                            parBandNr.Value = eenFilm.BandNr;
                            parInVoorraad.Value = eenFilm.InVoorraad;
                            parUitVoorraad.Value = eenFilm.UitVoorraad;
                            parTotaalVerhuurd.Value = eenFilm.TotaalVerhuurd;

                            if (comUpdate.ExecuteNonQuery() == 0)
                                nietGewijzigdeFilms.Add(eenFilm);
                        }
                        catch (Exception ex)
                        {
                            nietGewijzigdeFilms.Add(eenFilm);
                        }
                    }
                }
            }
            return nietGewijzigdeFilms;
        }
    }
    
}
