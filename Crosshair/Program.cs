using ShareCode;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;



public class CrosshairForm : Form
{
    // User-specified crosshair configuration
    private bool CenterDot;
    private float Length;
    private float Thickness;
    private float Gap;
    private int OutlineThickness;
    private int red;
    private int green;
    private int blue;

    private Color crosshairColor;
    private CrosshairInfo crosshairInfo;
    private string ToggleKey;

    public CrosshairForm()
    {
        ReadConfig();
        KeyboardHook.SetHook();
        KeyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;

        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.Magenta; // Unique color for transparency
        this.TransparencyKey = this.BackColor; // Make the unique color transparent
        this.TopMost = true; // Always on top
        this.StartPosition = FormStartPosition.CenterScreen;
        this.DoubleBuffered = true;
        crosshairColor = Color.FromArgb(red, green, blue);

        int formSize = 200;
        this.Width = formSize;
        this.Height = formSize;

        // Position the form at the center of the screen
        Rectangle screenRectangle = Screen.PrimaryScreen.Bounds;
        this.Location = new Point((screenRectangle.Width - this.Width) / 2,
                                  (screenRectangle.Height - this.Height) / 2);
    }

    private void KeyboardHook_OnKeyPressed(object sender, KeyEventArgs e)
    {
        if (ToggleKey != null && e.KeyCode == GetToggleKeyCode())
        {
            this.Visible = !this.Visible; // Toggle visibility
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        KeyboardHook.UnhookWindowsHookEx(); // Unhook when form closes
    }

    private void ReadConfig()
    {
        string configPath = "config.txt";
       //string outputPath = "decodedColors.txt";
        if (System.IO.File.Exists(configPath))
        {
            string[] lines = System.IO.File.ReadAllLines(configPath);
            
            foreach (string line in lines)
            {
                if (line.StartsWith("ShareCode="))
                {
                    string[] parts = line.Split('=');
                    if (parts.Length > 1)
                    {
                        string shareCode = parts[1].Trim();
                        if (!string.IsNullOrWhiteSpace(shareCode))
                        {
                            try
                            {
                                CrosshairInfo info = CrosshairInfo.Decode(ShareCode.ShareCode.Decode(shareCode));
                                CenterDot = info.HasCenterDot;
                                Length = info.Length;
                                Thickness = info.Thickness;
                                Gap = info.Gap;
                                OutlineThickness = info.HasOutline ? (int)info.Outline : 0;
                                red = info.Red;
                                green = info.Green;
                                blue = info.Blue;
                                crosshairColor = Color.FromArgb(red, green, blue);

                                //using (StreamWriter writer = new StreamWriter(outputPath))
                                //{
                                //    writer.WriteLine($"Red: {red}");
                                //    writer.WriteLine($"Green: {green}");
                                //    writer.WriteLine($"Blue: {blue}");
                                //}

                            }
                            catch
                            {
                                Application.Exit();
                            }
                        }
                    }
                }
                else if (line.StartsWith("ToggleKey="))
                {
                    string[] parts = line.Split('=');
                    if (parts.Length > 1)
                    {
                        string keyString = parts[1].Trim();
                        try
                        {
                            Keys key = (Keys)Enum.Parse(typeof(Keys), keyString, true);
                            ToggleKey = key.ToString(); // Store the key as string
                        }
                        catch
                        {
                            // Invalid key
                            ToggleKey = "None";
                        }
                    }
                    else
                    {
                        //Exit if no toggle key is provided
                        Application.Exit();
                    }
                }
                else
                {
                    //Exit if no ToggleKey line is found
                    Application.Exit();
                }

            }

        }
    }
    public Keys GetToggleKeyCode()
    {
        // Convert the first character to uppercase to match enum naming convention
        string formattedKey = char.ToUpper(ToggleKey[0]) + ToggleKey.Substring(1).ToLower();

        // Attempt to parse the formatted key string to a Keys enum
        if (Enum.TryParse<Keys>(formattedKey, out Keys keyCode))
        {
            return keyCode;
        }
        return Keys.None; // Return None if parsing fails
    }


    protected override CreateParams CreateParams
    {
        get
        {
            const int WS_EX_TRANSPARENT = 0x20;
            const int WS_EX_LAYERED = 0x80000;
            const int WS_EX_TOPMOST = 0x8;

            CreateParams cp = base.CreateParams;
            cp.ExStyle |= WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOPMOST;
            return cp;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        DrawCrosshair(e.Graphics, this.Width / 2, 20 + this.Height / 2);
       
    }

        //The following two functions are for the special case
        private void DrawEvenLengthLine(Graphics g, int x1, int y1, int x2, int y2, int thickness)
        {
            using (Pen pen = new Pen(Color.Black, thickness))
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        private void DrawOddLengthLine(Graphics g, int x1, int y1, int x2, int y2, int thickness)
        {
            if (x1 == x2)
            {
                y1 += 1;
            
            }
            else
            {
                x1 += 1;
            }

            using (Pen pen = new Pen(Color.Black, thickness))
            {
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }




    private void DrawLineWithOutline(Graphics g, int x1, int y1, int x2, int y2, int thickness, int outlineThickness)
    {
        // Determine if the line is vertical or horizontal
        bool isVertical = x1 == x2;

        // Calculate the length of the line
        int lineLength = isVertical ? Math.Abs(y2 - y1) : Math.Abs(x2 - x1);

        // Calculate dimensions and position of the outline
        int outlineWidth, outlineHeight, outlineX, outlineY;

        if (isVertical)
        {   
            // Vertical line adjustments
            outlineWidth = thickness + (outlineThickness * 2);
            outlineHeight = lineLength + (outlineThickness * 2) + (thickness == 2 ? 0 : 1);
            outlineX = x1 - outlineThickness - (thickness % 2 == 0 ? 1 : 0);
            outlineY = Math.Min(y1, y2) - outlineThickness;
            if (thickness == 3)
            {
                outlineHeight--;
                outlineX--;
            }
        }
        else
        {
            // Horizontal line adjustments
            outlineWidth = lineLength + (outlineThickness * 2) + (thickness == 2 ? 0 : 1);
            outlineHeight = thickness + (outlineThickness * 2);
            outlineX = x1 - (0) - outlineThickness;
            outlineY = y1 - outlineThickness - (thickness % 2 == 0 ? 1 : 0);
            if (thickness == 3)
            {
                outlineWidth--;
                outlineY--;
            }
        }
        //If we are to do a thickness
        if (outlineThickness > 0)
        {
            using (Brush outlineBrush = new SolidBrush(Color.Black))
            {

                g.FillRectangle(outlineBrush, outlineX, outlineY, outlineWidth, outlineHeight);
            }

        }

        // Draw line
        using (Pen pen = new Pen(crosshairColor, thickness))
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
    }

    private void DrawDotWithOutline(Graphics g, int x, int y, int thickness, int outlineThickness, Color dotColor)
    {
        // Set outline color to black if dot is enabled, otherwise transparent
        Color outlineColor = CenterDot ? Color.Black : Color.Transparent;

        // Draw outline
        using (Brush outlineBrush = new SolidBrush(outlineColor))
        {
            g.FillRectangle(outlineBrush, x - thickness / 2 - outlineThickness, y - thickness / 2 - outlineThickness, thickness + 2 * outlineThickness, thickness + 2 * outlineThickness);
        }

        // Draw dot
        using (Brush dotBrush = new SolidBrush(dotColor))
        {
            g.FillRectangle(dotBrush, x - thickness / 2, y - thickness / 2, thickness, thickness);
        }
    }


    public void DrawCrosshair(Graphics g, int centerX, int centerY)
    {
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

        // Convert input values to pixel values
        int pixelLength = CrosshairSettings.GetPixelLength(Length);
        int pixelThickness = CrosshairSettings.GetPixelThickness(Thickness);
        int pixelGap = CrosshairSettings.GetPixelGap(Gap);

        // Adjust the total distance from the center to where the line starts
        int totalDistance = pixelGap + 2;
        int adjustedLength = (pixelThickness == 1) ? pixelLength - 1 : pixelLength;

        // Apply correction for odd thickness greater than 1 (e.g., 3 pixels)
        int rightBottomOffset = (pixelThickness > 1 && pixelThickness % 2 != 0) ? 1 : 0;


        if (pixelLength == 0 && OutlineThickness > 0) // Special case for length 0
        {
            // Line length determined by the thickness input
            int specialLineLength = CrosshairSettings.GetPixelLength(Thickness) + 3;
            int lineHalfLength = specialLineLength / 2;
            int lineHalfLengthRounded = (specialLineLength + 1) / 2; // Adjust for odd lengths

            // Determine if line length is odd
            bool isLineLengthOdd = specialLineLength % 2 == 0;

            // Choose the appropriate method based on line length
            Action<Graphics, int, int, int, int, int> drawLineMethod;
            if (isLineLengthOdd)
            {
                drawLineMethod = DrawOddLengthLine;

            }
            else
            {
                drawLineMethod = DrawEvenLengthLine;
            }

            // Draw each line using the chosen method
            drawLineMethod(g,
                           centerX - lineHalfLength,
                           centerY - pixelGap - lineHalfLength + (isLineLengthOdd ? 1 : 0),
                           centerX + lineHalfLength,
                           centerY - pixelGap - lineHalfLength + (isLineLengthOdd ? 1 : 0),
                           2); // Top

            drawLineMethod(g,
                           centerX - lineHalfLength,
                           centerY + pixelGap + lineHalfLength,
                           centerX + lineHalfLength,
                           centerY + pixelGap + lineHalfLength,
                           2); // Bottom

            drawLineMethod(g,
                           centerX - pixelGap - lineHalfLength + (isLineLengthOdd ? 1 : 0),
                           centerY - lineHalfLength,
                           centerX - pixelGap - lineHalfLength + (isLineLengthOdd ? 1 : 0),
                           centerY + lineHalfLength,
                           2); // Left

            drawLineMethod(g,
                           centerX + pixelGap + lineHalfLength,
                           centerY - lineHalfLength,
                           centerX + pixelGap + lineHalfLength,
                           centerY + lineHalfLength,
                           2); // Right

            DrawDotWithOutline(g, centerX, centerY, pixelThickness, OutlineThickness, CenterDot ? crosshairColor : Color.Transparent);

        }


        else
        {

            DrawLineWithOutline(g, centerX - totalDistance - adjustedLength, centerY, centerX - totalDistance, centerY, pixelThickness, OutlineThickness); // Left
            DrawLineWithOutline(g, centerX + totalDistance + rightBottomOffset, centerY, centerX + totalDistance + adjustedLength + rightBottomOffset, centerY, pixelThickness, OutlineThickness); // Right
            DrawLineWithOutline(g, centerX, centerY - totalDistance - adjustedLength, centerX, centerY - totalDistance, pixelThickness, OutlineThickness); // Top
            DrawLineWithOutline(g, centerX, centerY + totalDistance + rightBottomOffset, centerX, centerY + totalDistance + adjustedLength + rightBottomOffset, pixelThickness, OutlineThickness); // Bottom

            DrawDotWithOutline(g, centerX, centerY, pixelThickness, OutlineThickness, CenterDot ? crosshairColor : Color.Transparent);
        }


    }




    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        CrosshairForm crosshairForm = new CrosshairForm();


        int canvasWidth = 1920;
        int canvasHeight = 1080;
            
        using (Bitmap bmp = new Bitmap(canvasWidth, canvasHeight))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent); // Set the background to be transparent

                // Draw the crosshair at the center of the canvas
                crosshairForm.DrawCrosshair(g, canvasWidth / 2, canvasHeight / 2);

                // Save the bitmap as a PNG file
                bmp.Save("crosshair.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        Application.Run(crosshairForm);
    }



}
