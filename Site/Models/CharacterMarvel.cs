using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;

namespace Site.Models
{
    public class CharacterMarvel : ModelBaseMarvel<CharacterMarvel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public string Thumbnail { get; set; }

        public List<CharacterMarvel> GetCharactersMarvel(string textSearch = "")
        {
            var url = "characters?orderBy=name&";

            if (textSearch != "")
                url += "nameStartsWith=" + textSearch + "&";

            var lst = GetAll(url);

            var ltsCharacters = new List<CharacterMarvel>();

            if (lst == null)
                return ltsCharacters;

            foreach (dynamic item in lst)
            {
                ltsCharacters.Add(new CharacterMarvel()
                {
                    Id = item.id,
                    Name = item.name,
                    Description = item.description,
                    Modified = item.modified,
                    Thumbnail = (item.thumbnail.path + "." + item.thumbnail.extension)
                });
            }

            return ltsCharacters;
        }
        public CharacterMarvel GetCharacterMarvel(int idCharacter)
        {
            var url = "characters/" + idCharacter + "?";

            var obj = GetAll(url);

            if (obj == null)
                return new CharacterMarvel();

            var objCharacter = new CharacterMarvel() {
                Id = obj.First.id,
                Name = obj.First.name,
                Description = obj.First.description,
                Modified = obj.First.modified,
                Thumbnail = (obj.First.thumbnail.path + "." + obj.First.thumbnail.extension)
            };

            return objCharacter;
        }

    }
}