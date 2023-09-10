using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Northwind.DataAccess.SqlServer
{
    /// <summary>
    /// Class WriterToXml.
    /// </summary>
    internal class WriterToXml
    {
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriterToXml"/> class.
        /// </summary>
        /// <param name="stream">A stream.</param>
        /// <exception cref="ArgumentNullException">Throw when stream is null.</exception>
        public WriterToXml(Stream stream) => this.stream = stream ?? throw new ArgumentNullException(nameof(stream));

        /// <summary>
        /// Converts a sequence of values to xml.
        /// </summary>
        /// <param name="collection">A collection/>.</param>
        /// <param name="collectionName">A collection name/>.</param>
        /// <param name="itemName">A item name/>.</param>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <returns>A task.</returns>
        public async Task GetXmlAsync<T>(IEnumerable<T> collection, string collectionName, string itemName)
        {
            var settings = new XmlWriterSettings();
            settings.Async = true;
            await using var writer = XmlWriter.Create(this.stream, settings);
            await writer.WriteStartElementAsync(null, collectionName, null);
            foreach (var item in collection)
            {
                await writer.WriteElementStringAsync(null, itemName, null, Convert.ToString(item, CultureInfo.InvariantCulture));
            }

            await writer.WriteEndElementAsync();
            await writer.FlushAsync();
        }

        /// <summary>
        /// Converts a sequence of strings to xml.
        /// </summary>
        /// <param name="collection">A collection/>.</param>
        /// <param name="collectionName">A collection name/>.</param>
        /// <param name="itemName">A item name/>.</param>
        /// <returns>A task.</returns>
        public async Task GetXmlAsync(IEnumerable<string> collection, string collectionName, string itemName)
        {
            var settings = new XmlWriterSettings();
            settings.Async = true;
            await using var writer = XmlWriter.Create(this.stream, settings);
            await writer.WriteStartElementAsync(null, collectionName, null);
            foreach (var item in collection)
            {
                await writer.WriteElementStringAsync(null, itemName, null, item);
            }

            await writer.WriteEndElementAsync();
            await writer.FlushAsync();
        }
    }
}
