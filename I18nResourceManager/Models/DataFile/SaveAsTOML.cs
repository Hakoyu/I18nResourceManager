using HKW.HKWTOML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I18nResourceManager.Models.DataFile;

internal class SaveAsTOML : IDataFileSave
{
    public void SaveToFile(string filePath, I18nResource resource)
    {
        var resourcePath = Path.Combine(filePath, resource.Name);
        Directory.CreateDirectory(resourcePath);
        foreach (var cultureName in resource.CultureNames)
        {
            var toml = new TomlTable();
            foreach (var data in resource.Datas)
            {
                toml.Add(
                    data.Id,
                    new TomlString(data.Texts[cultureName].Value)
                    {
                        Comment = data.Texts[cultureName].Comment
                    }
                );
            }
            toml.SaveToFile(Path.Combine(resourcePath, cultureName));
        }
    }
}
