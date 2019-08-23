using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Site.Models
{
    public class ComicMarvel : ModelBaseMarvel<ComicMarvel>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }

        public List<ComicMarvel> GetComicMarvel(int idCharacter)
        {
            var url = "characters/" + idCharacter + "/comics?orderBy=title&";
            var response = GetAll(url);

            var lst = GetAll(url);
            var ltsComiics = new List<ComicMarvel>();

            if (lst == null)
                return ltsComiics;

            foreach (dynamic item in lst)
            {
                ltsComiics.Add(new ComicMarvel()
                {
                    Id = item.id,
                    Title = item.title,
                    Thumbnail = (item.thumbnail.path + "." + item.thumbnail.extension)
                });
            }

            return ltsComiics;
        }

    }
}