namespace WinFormsAtBitmarck;

public class WindowManager : IWindowManager
{
    private readonly IServiceProvider _services;

    public WindowManager(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public void Show<TForm>() where TForm : Form
    {
        var form = (TForm) _services.GetService(typeof(TForm));
        form!.Show();
    }
}