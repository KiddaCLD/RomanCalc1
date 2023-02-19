using Avalonia.Controls;
using Avalonia.VisualTree;

namespace TestCalcTraining
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var app = AvaloniaApp.GetApp();
            var mainWindow = AvaloniaApp.GetMainWindow();

            await Task.Delay(100);

            var button = mainWindow.GetVisualDescendants().OfType<Button>().First(b => b.Name == "buttonI");
            var textBlock = mainWindow.GetVisualDescendants().OfType<TextBlock>().First(c => c.Name == "textInput");

            button.Command.Execute(button.CommandParameter); // Имитирует нажатие пользователем ЛКМ

            await Task.Delay(50);
            var textBlockContent = textBlock.Text;

            Assert.True(textBlockContent.Equals("I"));
        }

        [Fact]
        public async void Test2()
        {
            var app = AvaloniaApp.GetApp();
            var mainWindow = AvaloniaApp.GetMainWindow();

            await Task.Delay(100);

            var button = mainWindow.GetVisualDescendants().OfType<Button>().First(b => b.Name == "buttonI");
            var buttonClear = mainWindow.GetVisualDescendants().OfType<Button>().First(b => b.Name == "ButtonClear");
            var textBlock = mainWindow.GetVisualDescendants().OfType<TextBlock>().First(c => c.Name == "textInput");

            button.Command.Execute(button.CommandParameter);
            buttonClear.Command.Execute(button.CommandParameter);

            await Task.Delay(50);

            var textBlockContent = textBlock.Text;

            Assert.True(textBlockContent.Equals(""));
        }
    }
}