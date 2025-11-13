public interface IModular
{
    float Module();
}
class ComplexNumber : ICloneable, IEquatable<ComplexNumber>, IModular
{
    private float re;
    public float Re { get { return re; } set { re = value; } }
    private float im;
    public float Im { get { return im; } set { im = value; } }

    public ComplexNumber(float re, float im)
    {
        Re = re;
        Im = im;
    }

    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        float newRe = a.Re + b.Re;
        float newIm = a.Im + b.Im;
        
        ComplexNumber result = new ComplexNumber(newRe, newIm);
        return result;
    }

    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        float newRe = a.Re - b.Re;
        float newIm = a.Im - b.Im;

        ComplexNumber result = new ComplexNumber(newRe, newIm);
        return result;
    }

    public static ComplexNumber operator -(ComplexNumber a)
    {
        ComplexNumber result = new(a.Re, -a.Im);
        return result;
    }

    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        float first_bracket = (a.re *  b.re) - (a.im * b.im);
        float second_bracket = (a.re * b.im) + (a.im * b.re);

        ComplexNumber result = new ComplexNumber(first_bracket, second_bracket);
        return result;
    }

    public override string ToString()
    {
        if (im < 0)
        {
            float absIm = Math.Abs(im);
            return re.ToString() + " - " + absIm  + "i";
        }
        else
        {
            return re.ToString() + " + " + im.ToString() + "i";
        }
    }
    public object Clone() { 
        return new ComplexNumber(this.re, this.im);
    }

    public bool Equals(ComplexNumber other) {
        if(other is null)
        {
            return false;
        }

        return this.re == other.re && this.im == other.im;
    }

    public float Module()
    {
        return MathF.Sqrt((re* re) + (im * im));
    }
}

class Program
{
    public static void Main(string[] args)
    {
        ComplexNumber complexNumber = new ComplexNumber(6, 7);
        ComplexNumber complexNumber1 = new ComplexNumber(6, -7);
        ComplexNumber complexNumber2  = complexNumber + complexNumber1;
        ComplexNumber complexNumber3 = complexNumber2 - complexNumber1;

        ComplexNumber complexNumber4 = (ComplexNumber)complexNumber3.Clone();

        string out1 = complexNumber.ToString();
        string out2 = complexNumber1.ToString();
        string out3 = complexNumber2.ToString();
        string out4 = complexNumber3.ToString();
        
        float complexNumberModule = complexNumber.Module();
        complexNumber2 = -complexNumber2;

        string out5 = complexNumber2.ToString();

        bool result = complexNumber.Equals(complexNumber1);

        Console.WriteLine(out1);
        Console.WriteLine(out2);
        Console.WriteLine(out3);
        Console.WriteLine(out4);
        Console.WriteLine(out5);
        Console.WriteLine(result);
        Console.WriteLine(complexNumberModule);
    }
}