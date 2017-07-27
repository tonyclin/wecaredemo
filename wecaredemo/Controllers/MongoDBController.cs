using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Bson;
using System.Net;
using MongoDB.Driver;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace wecaredemo
{

    public class MongoDBController : Controller
    {
        [HttpGet]
        [Route("api/crm/getRule")]
        public IActionResult GetRule(string id)
        {
            try
            {
                var client = new MongoDB.Driver.MongoClient();
                var db = client.GetDatabase("crm");
                IMongoCollection<BsonDocument> cols = db.GetCollection<BsonDocument>("QNA");
                var model = cols.AsQueryable().ToList().Select(c =>
                    new
                    {
                        qnaid = c["qnaid"].AsString,
                        input = c["input"].AsString,
                        matchto = c["matchto"].AsString,
                        questids = c["questids"].AsBsonArray.Select(p => p.AsInt32).ToArray(),
                        children = c["children"].AsBsonArray.Select(p => p.AsString).ToArray()
                    });


                cols = db.GetCollection<BsonDocument>("quests");
                var quests = cols.AsQueryable().ToList().Select(c =>
                  new
                  {
                      questID = c["docId"].AsString,
                      question = c["question"].AsString
                  });
                return Ok(new { qna = model, questions = quests });
            }
            catch (Exception ex)
            {

            }
            return NotFound();

        }
    }
}
