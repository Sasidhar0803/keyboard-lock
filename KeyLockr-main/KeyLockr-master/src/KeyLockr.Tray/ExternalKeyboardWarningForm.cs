using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeyLockr.Tray;

public sealed class ExternalKeyboardWarningForm : Form
{
    private readonly System.Windows.Forms.Timer _timer;
    private readonly Label _countdownLabel;
    private int _remainingSeconds;

    public ExternalKeyboardWarningForm(string warningMessage, int countdownSeconds = 10)
    {
        _remainingSeconds = Math.Max(3, countdownSeconds);

        Text = "Confirm Lock Built-in Keyboard";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        TopMost = true;
        ClientSize = new Size(450, 200);

        var messageLabel = new Label
        {
            AutoSize = false,
            Text = warningMessage + "\n\nIf you continue, please ensure your external keyboard is working properly.",
            Dock = DockStyle.Top,
            Height = 100,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _countdownLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font(Font, FontStyle.Bold)
        };

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(10),
            Height = 60
        };

        var continueButton = new Button
        {
            Text = "Continue Lock",
            DialogResult = DialogResult.OK,
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };

        var cancelButton = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };

        buttonsPanel.Controls.Add(continueButton);
        buttonsPanel.Controls.Add(cancelButton);

        Controls.Add(buttonsPanel);
        Controls.Add(_countdownLabel);
        Controls.Add(messageLabel);

        AcceptButton = cancelButton;
        CancelButton = cancelButton;

        _timer = new System.Windows.Forms.Timer { Interval = 1000 };
        _timer.Tick += (_, _) => UpdateCountdown();
        Shown += (_, _) => { UpdateCountdown(); _timer.Start(); };
        FormClosed += (_, _) => _timer.Stop();
    }

    private void UpdateCountdown()
    {
        _countdownLabel.Text = $"Will automatically cancel in {_remainingSeconds} second(s)";
        if (_remainingSeconds <= 0)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }

        _remainingSeconds--;
    }
}