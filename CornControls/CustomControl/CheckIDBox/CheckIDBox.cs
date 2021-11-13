using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CornControls.CustomControl
{
    public class CheckIDBox : ComboBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CheckIDBox));
        public static readonly DependencyProperty ItemCornerRadiusProperty = DependencyProperty.Register(nameof(ItemCornerRadius), typeof(CornerRadius), typeof(CheckIDBox));

        public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register("NormalColor", typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register("HoverColor", typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty FocusedColorProperty = DependencyProperty.Register(nameof(FocusedColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ItemBorderFocusedColorProperty = DependencyProperty.Register(nameof(ItemBorderFocusedColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty ItemNormalColorProperty = DependencyProperty.Register(nameof(ItemNormalColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty ItemHoverColorProperty = DependencyProperty.Register(nameof(ItemHoverColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty ItemSelectedColorProperty = DependencyProperty.Register(nameof(ItemSelectedColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty ItemHoverAndSelectedColorProperty = DependencyProperty.Register(nameof(ItemHoverAndSelectedColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty ItemSelectedForegroundProperty = DependencyProperty.Register(nameof(ItemSelectedForeground), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty DropDownColorProperty = DependencyProperty.Register(nameof(DropDownColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(Geometry), typeof(CheckIDBox));

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty IconHoverColorProperty = DependencyProperty.Register(nameof(IconHoverColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(CheckIDBox));
        public static readonly DependencyProperty IconPadProperty = DependencyProperty.Register("IconPad", typeof(GridLength), typeof(CheckIDBox));

        public static readonly DependencyProperty FocusedPaddingProperty = DependencyProperty.Register(nameof(FocusedPadding), typeof(Thickness), typeof(CheckIDBox));
        public static readonly DependencyProperty NormalPaddingProperty = DependencyProperty.Register(nameof(NormalPadding), typeof(Thickness), typeof(CheckIDBox));

        public static readonly DependencyProperty OutterColorProperty = DependencyProperty.Register(nameof(OutterColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty FocusedOutterColorProperty = DependencyProperty.Register(nameof(FocusedOutterColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register(nameof(ReadOnlyBackground), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(nameof(DisabledOpacity), typeof(double), typeof(CheckIDBox), new PropertyMetadata(0.8));
        public static readonly DependencyProperty DisabledColorProperty = DependencyProperty.Register(nameof(DisabledColor), typeof(Brush), typeof(CheckIDBox), new PropertyMetadata(Brushes.Gray));

        [Browsable(true), Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public CornerRadius ItemCornerRadius
        {
            get => (CornerRadius)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        // Control color
        [Browsable(true), Category("Appearance")]
        public Brush NormalColor
        {
            get => (Brush)GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush HoverColor
        {
            get => (Brush)GetValue(HoverColorProperty);
            set => SetValue(HoverColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush FocusedColor
        {
            get => (Brush)GetValue(FocusedColorProperty);
            set => SetValue(FocusedColorProperty, value);
        }

        // Item color
        [Browsable(true), Category("Appearance")]
        public Brush ItemBorderFocusedColor
        {
            get => (Brush)GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ItemNormalColor
        {
            get => (Brush)GetValue(ItemNormalColorProperty);
            set => SetValue(ItemNormalColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ItemHoverColor
        {
            get => (Brush)GetValue(ItemHoverColorProperty);
            set => SetValue(ItemHoverColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ItemSelectedColor
        {
            get => (Brush)GetValue(ItemSelectedColorProperty);
            set => SetValue(ItemSelectedColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush ItemHoverAndSelectedColor
        {
            get => (Brush)GetValue(ItemHoverAndSelectedColorProperty);
            set => SetValue(ItemHoverAndSelectedColorProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Brush ItemSelectedForeground
        {
            get => (Brush)GetValue(ItemSelectedForegroundProperty);
            set => SetValue(ItemSelectedForegroundProperty, value);
        }

        // Dropdown color
        [Browsable(true), Category("Appearance")]
        public Brush DropDownColor
        {
            get => (Brush)GetValue(DropDownColorProperty);
            set => SetValue(DropDownColorProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Geometry Path
        {
            get => (Geometry)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public double IconColor
        {
            get => (double)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public double IconHoverColor
        {
            get => (double)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public GridLength IconPad
        {
            get => (GridLength)GetValue(IconPadProperty);
            set => SetValue(IconPadProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Thickness FocusedPadding
        {
            get => (Thickness)GetValue(FocusedPaddingProperty);
            set => SetValue(FocusedPaddingProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Thickness NormalPadding
        {
            get => (Thickness)GetValue(NormalPaddingProperty);
            set => SetValue(NormalPaddingProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Brush OutterColor
        {
            get => (Brush)GetValue(OutterColorProperty);
            set => SetValue(OutterColorProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public Brush FocusedOutterColor
        {
            get => (Brush)GetValue(FocusedOutterColorProperty);
            set => SetValue(FocusedOutterColorProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public Brush ReadOnlyBackground
        {
            get => (Brush)GetValue(ReadOnlyBackgroundProperty);
            set => SetValue(ReadOnlyBackgroundProperty, value);
        }
        [Browsable(true), Category("Appearance")]
        public double DisabledOpacity
        {
            get => (double)GetValue(DisabledOpacityProperty);
            set
            {
                value = Math.Clamp(value, 0, 1);
                SetValue(DisabledOpacityProperty, value);
            }
        }
        [Browsable(true), Category("Appearance")]
        public Brush DisabledColor
        {
            get => (Brush)GetValue(DisabledColorProperty);
            set => SetValue(DisabledColorProperty, value);
        }

        // For testing purpose

        private TextBox _textBox = null;

        public static readonly DependencyProperty MessageTextProperty = DependencyProperty.Register(nameof(MessageText), typeof(string), typeof(CheckIDBox), new FrameworkPropertyMetadata("Empty", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty IDTextProperty = DependencyProperty.Register(nameof(IDText), typeof(string), typeof(CheckIDBox), new FrameworkPropertyMetadata("Empty", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty NameTextProperty = DependencyProperty.Register(nameof(NameText), typeof(string), typeof(CheckIDBox), new FrameworkPropertyMetadata("Empty", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty ProfilePictureProperty = DependencyProperty.Register(nameof(ProfilePicture), typeof(ImageSource), typeof(CheckIDBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [Browsable(true), Category("Appearance")]
        public string MessageText
        {
            get => (string)GetValue(MessageTextProperty);
            set => SetValue(MessageTextProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public string IDText
        {
            get => (string)GetValue(IDTextProperty);
            set => SetValue(IDTextProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public string NameText
        {
            get => (string)GetValue(NameTextProperty);
            set => SetValue(NameTextProperty, value);
        }

        [Browsable(true), Category("Appearance")]
        public ImageSource ProfilePicture
        {
            get => (ImageSource)GetValue(ProfilePictureProperty);
            set => SetValue(ProfilePictureProperty, value);
        }

        static CheckIDBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckIDBox), new FrameworkPropertyMetadata(typeof(CheckIDBox)));
        }

        public override void OnApplyTemplate()
        {
            _textBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
            if (_textBox != null)
            {
                _textBox.TextChanged += (s, e) =>
                {
                    IsDropDownOpen = false;
                };
            }

            base.OnApplyTemplate();
        }
    }
}
