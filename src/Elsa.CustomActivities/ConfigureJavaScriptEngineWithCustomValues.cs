using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Extensions;
using NetBox.Extensions;

namespace Elsa.CustomActivities
{
    public class ConfigureJavaScriptEngineWithCustomValues : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public ConfigureJavaScriptEngineWithCustomValues(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;

            var dictionary = new Dictionary<string, object>()
            {
                { "Stage1_First", 1 },
                { "Stage1_Second", 2 },
                { "Stage2_First", 3 },
                { "Stage1_Second", 4 },
                { "Stage1_Third", 5 },
            };

            //engine.SetValue("dataDictionary", dictionary);
            engine.RegisterType<DataDictionary>();
            return Task.CompletedTask;
        }

        //public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        //{
        //    var output = notification.Output;

        //    output.AppendLine("declare function contentTypeFromFileName(fileName: string): string");

        //    return Task.CompletedTask;
        //}
        public class DataDictionary
        {
            public string TestProperty { get; set; }
            public Dictionary<string, int> Items { get; set; }
            //{
                //get
                //{
                //    return new Dictionary<string, object>()
                //    {
                //        { "Stage1_First", 1 },
                //        { "Stage1_Second", 2 },
                //        { "Stage2_First", 3 },
                //        { "Stage2_Second", 4 },
                //        { "Stage2_Third", 5 },
                //    };
                //}
            //}
        }

        public async Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare interface DataDictionary {");
            var test = new DataDictionary();
            var items = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);
            //var distinctItems = items.GroupBy(x => new {x.Group.Name, x.Name}).Select(x => x.First()).ToList();
            var distinctItems = items.Take(15);
            try
            {
                Dictionary<string, int> itemsDictionary =
                    distinctItems.ToDictionary(x => $"{x.Group.Name} - {x.Name}", x => x.Id);
                test.Items = itemsDictionary;

            }
            catch (Exception ex)
            {
                var x = 0;
            }

            foreach (var property in test.Items)
            {
                var interfaceActivity = $"{property.Key}: string";
                output.AppendLine($"{interfaceActivity};");
            }
            output.AppendLine("}");
            output.AppendLine("declare const dataDictionary: DataDictionary;");
        }
    }
}
