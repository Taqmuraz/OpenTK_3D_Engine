using System.Drawing;
using System.Windows.Forms;

namespace Engine
{
	public interface IInputHandler
	{
		void OnKeyDown(Keys key);
		void OnKeyUp(Keys key);
		void OnMouseDown(PointF point, int button);
		void OnMouseMove(PointF point);
		void OnMouseUp(PointF point, int button);
	}
}
