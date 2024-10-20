using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace GutenbergAuthorBookDownloader
{
    public partial class Form1 : Form
    {
        private const string SearchUrl = "https://www.gutenberg.org/ebooks/search/?query=";

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            string authorName = textBoxAuthorName.Text.Trim();

            if (string.IsNullOrEmpty(authorName))
            {
                MessageBox.Show("Пожалуйста, введите имя и фамилию автора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var books = await GetBooksByAuthorAsync(authorName);
                await DownloadBooksAsync(books);
                MessageBox.Show("Скачивание завершено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<Book>> GetBooksByAuthorAsync(string authorName)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(SearchUrl + Uri.EscapeDataString(authorName));
                var doc = new HtmlAgilityPack.HtmlDocument(); 
                doc.LoadHtml(response);

                var bookNodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'booklink')]");

                if (bookNodes == null)
                    throw new Exception("Не удалось найти книги для указанного автора.");

                List<Book> books = new List<Book>();
                foreach (var bookNode in bookNodes)
                {
                    var titleNode = bookNode.SelectSingleNode(".//h3/a");
                    if (titleNode != null)
                    {
                        string title = titleNode.InnerText.Trim();
                        string bookUrl = "https://www.gutenberg.org" + titleNode.GetAttributeValue("href", "");
                        books.Add(new Book { Title = title, Url = bookUrl });
                    }
                }

                return books;
            }
        }

        private async Task DownloadBooksAsync(List<Book> books)
        {
            using (HttpClient client = new HttpClient())
            {
                string downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GutenbergBooks");

                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }

                foreach (var book in books)
                {
                    var response = await client.GetStringAsync(book.Url);
                    var doc = new HtmlAgilityPack.HtmlDocument(); 
                    doc.LoadHtml(response);

                    var textNode = doc.DocumentNode.SelectSingleNode("//a[contains(@title, 'Plain Text')]");
                    if (textNode != null)
                    {
                        string downloadUrl = "https://www.gutenberg.org" + textNode.GetAttributeValue("href", "");
                        var bookContent = await client.GetStringAsync(downloadUrl);

                        string safeTitle = Path.GetInvalidFileNameChars().Aggregate(book.Title, (current, c) => current.Replace(c.ToString(), ""));
                        string filePath = Path.Combine(downloadFolder, $"{safeTitle}.txt");
                        File.WriteAllText(filePath, bookContent);
                    }
                }
            }
        }

    }

    public class Book
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
