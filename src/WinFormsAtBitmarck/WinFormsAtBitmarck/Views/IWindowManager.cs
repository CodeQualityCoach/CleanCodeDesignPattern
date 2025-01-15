namespace WinFormsAtBitmarck;

public interface IWindowManager
{
    void Show<TForm>() where TForm : Form;
}