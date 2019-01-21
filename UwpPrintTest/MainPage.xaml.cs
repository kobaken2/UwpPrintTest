using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace UwpPrintTest
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private PrintDocument printDocument;
		private IPrintDocumentSource printDocumentSource;

		public MainPage()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			this.printDocument = new PrintDocument();
			this.printDocumentSource = this.printDocument.DocumentSource;
			this.printDocument.GetPreviewPage += this.PrintDocument_GetPreviewPage;
			this.printDocument.Paginate += this.PrintDocument_Paginate;
			this.printDocument.AddPages += this.printDocument_AddPages;

			PrintManager.GetForCurrentView().PrintTaskRequested += this.MainPage_PrintTaskRequested;
		}

		private void printDocument_AddPages(object sender, AddPagesEventArgs e)
		{
			// 印刷(複数ページあるときはAddPageを複数回呼ぶ）
			this.printDocument.AddPage(this.image);
			this.printDocument.AddPage(this.image1);
			this.printDocument.AddPagesComplete();
		}

		private void PrintDocument_Paginate(object sender, PaginateEventArgs e)
		{
			// ここで印刷コンテンツを作ってページ数を設定する
			this.printDocument.SetPreviewPageCount(2, PreviewPageCountType.Intermediate);
		}

		private void PrintDocument_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
		{
			// プレビューを表示する
			this.printDocument.SetPreviewPage(e.PageNumber, this.image);
		}

		private void MainPage_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
		{
			// プリントのタスクを登録する
			args.Request.CreatePrintTask("Sample", req =>
			{
				req.SetSource(this.printDocumentSource);
			});
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			await PrintManager.ShowPrintUIAsync();

		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			PrintManager.GetForCurrentView().PrintTaskRequested -= this.MainPage_PrintTaskRequested;
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			//var fileUri = "ms-appx:///image/Surface_Pro_3_pvrgaa5551-img600x450-1460350433irka0i29044.jpg";

			// 印刷(複数ページあるときはAddPageを複数回呼ぶ）
			//var bitmap = new BitmapImage(new Uri(fileUri));
			this.image.Source = new BitmapImage(new Uri("ms-appx:///image/Surface_Pro_3_pvrgaa5551-img600x450-1460350433irka0i29044.jpg", UriKind.RelativeOrAbsolute));
			this.printDocument.AddPage(this.image);
			this.printDocument.AddPagesComplete();

		}
	}
}
