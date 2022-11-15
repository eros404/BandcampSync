using Eros404.BandcampSync.Core.Services;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.AppSettings.Services
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly Assembly _executingAssembly;
        private readonly IOptionsMonitor<T> _options;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(
            Assembly executingAssembly,
            IOptionsMonitor<T> options,
            string section,
            string file)
        {
            _options = options;
            _section = section;
            _file = file;
            _executingAssembly = executingAssembly;
        }

        public T Value => _options.CurrentValue;
        public T Get(string? name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var physicalPath = Path.Combine(Path.GetDirectoryName(_executingAssembly.Location) ?? "", _file);

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            if (jObject == null)
                return;
            var sectionObject = jObject.TryGetValue(_section, out var section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : Value;
            if (sectionObject == null)
                return;
            applyChanges(sectionObject);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}
