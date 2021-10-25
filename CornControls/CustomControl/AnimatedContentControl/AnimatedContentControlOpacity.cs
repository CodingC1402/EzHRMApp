using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace CornControls.CustomControl
{
    public class AnimatedContentControlOpacity : AnimatedContentControl
    {
        protected override void BeginAnimateContentReplacement()
        {
            m_paintArea.Visibility = Visibility.Visible;
            m_mainContent.Visibility = Visibility.Hidden;

            m_paintArea.BeginAnimation(Shape.OpacityProperty, CreateAnimation(1, 0, 0, EasingMode.EaseIn, (s, e) => {
                m_paintArea.Visibility = Visibility.Hidden;
                Task.Delay((int)(OutDelay * 1000)).ContinueWith(t => {
                    Dispatcher.Invoke(() => {
                        m_mainContent.BeginAnimation(ContentPresenter.OpacityProperty, CreateAnimation(0, 1, 0, EasingMode.EaseIn));
                        m_mainContent.Visibility = Visibility.Visible;
                    });
                });
            }));
        }
    }
}
