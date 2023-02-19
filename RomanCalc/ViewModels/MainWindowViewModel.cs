using System.Collections.Generic;
using ReactiveUI;
using System.Linq;
using System.Reactive;

namespace RomanCalc.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string buttonText = "";
        string? LeftOperator = null;
        string? Command = null;
        string displayText = "";

        Dictionary<int, string> ra = new Dictionary<int, string>
        { 
            { 9  , "IX" },  { 5  , "V" },  { 4  , "IV" },  { 1  , "I" },
            { 90 , "XC" },  { 50 , "L" },  { 40 , "XL" },  { 10 , "X" },
            { 900, "CM" },  { 500, "D" },  { 400, "CD" },  { 100, "C" },
            { 1000, "M" }
        };

        int ToArabic(string number) => number.Length == 0 ? 0 : ra
        .Where(d => number.StartsWith(d.Value))
        .Select(d => d.Key + ToArabic(number.Substring(d.Value.Length)))
        .FirstOrDefault();
        
        string ToRoman(int number) => ra
            .Where(d => number >= d.Key)
            .Select(d => d.Value + ToRoman(number - d.Key))
            .FirstOrDefault();


        public MainWindowViewModel()
        {
            ButtonClickI = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickV = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickX = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickL = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickC = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickD = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickM = ReactiveCommand.Create<string, string>(str => ButtonText = ToRoman(ToArabic(ButtonText + str)));
            ButtonClickMathCommand = ReactiveCommand.Create<string>(command => ClickMathOperation(command));

        }

        public string ButtonText
        {
            get => buttonText;
            set
            {
                this.RaiseAndSetIfChanged(ref buttonText, value);

                if (ToArabic(buttonText) >= 4000)
                {
                    Error();
                }

            }
        }

        public string DisplayText
        {
            get => displayText;
            set
            {
                this.RaiseAndSetIfChanged(ref displayText, value);
                if (ToArabic(displayText) >= 4000)
                {
                    DisplayError();
                }
            }
        }


        public ReactiveCommand<string, string> ButtonClickI { get; }
        public ReactiveCommand<string, string> ButtonClickV { get; }
        public ReactiveCommand<string, string> ButtonClickX { get; }
        public ReactiveCommand<string, string> ButtonClickL { get; }
        public ReactiveCommand<string, string> ButtonClickC { get; }
        public ReactiveCommand<string, string> ButtonClickD { get; }
        public ReactiveCommand<string, string> ButtonClickM { get; }
        public ReactiveCommand<string, Unit> ButtonClickMathCommand { get; }
        public void OnClickButtonEquale()
        {
            if (LeftOperator is null) return;
            int resultNumber = Calculate(LeftOperator, buttonText, Command);
            DisplayText = ToRoman(resultNumber);
            LeftOperator = null;
            Command = null;
            ButtonText = "";
        }

        public void ClickMathOperation(string command)
        {
            if (LeftOperator is null)
            {
                LeftOperator = buttonText;
                Command = command;
            }
            else
            {
                OnClickButtonEquale();
                LeftOperator = DisplayText;
                Command = command;
            }
            ButtonText = "";
        }

        public void Error() => ButtonText = "#ERROR";

        public void DisplayError() => DisplayText = "#ERROR";

        public void Clear()
        {
            LeftOperator = null;
            DisplayText = "";
            ButtonText = "";
            Command = null;
        }

        public int Calculate(string? leftOperator, string? rightOperator, string? command)
        {
            int leftNumber = ToArabic(leftOperator);
            int rightNumber = ToArabic(rightOperator);
            int resultNumber = 0;

            switch (command)
            {
                case "*":
                    resultNumber = leftNumber * rightNumber;
                    break;
                case "/":
                    resultNumber = leftNumber / rightNumber;
                    break;
                case "-":
                    resultNumber = leftNumber - rightNumber;
                    break;
                case "+":
                    resultNumber = leftNumber + rightNumber;
                    break;
                default:
                    break;

            }
            return resultNumber;
        }

    }
}
