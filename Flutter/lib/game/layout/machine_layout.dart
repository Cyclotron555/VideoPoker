import 'dart:math' as math;
import 'dart:ui' as ui;

/// One fixed logical coordinate system for the Red Black Poker cabinet.
///
/// Every main-game asset and hit target is authored against [designWidth] x
/// [designHeight]. A real device receives one uniform scale factor, so X and Y
/// can never stretch independently.
class MachineLayout {
  const MachineLayout(this.viewportWidth, this.viewportHeight);

  static const double designWidth = 963.0;
  static const double designHeight = 1633.0;

  final double viewportWidth;
  final double viewportHeight;

  double get scale => math.min(
        viewportWidth / designWidth,
        viewportHeight / designHeight,
      );

  double get contentWidth => designWidth * scale;
  double get contentHeight => designHeight * scale;

  double get offsetX => (viewportWidth - contentWidth) / 2.0;
  double get offsetY => (viewportHeight - contentHeight) / 2.0;

  ui.Rect get contentRect =>
      ui.Rect.fromLTWH(offsetX, offsetY, contentWidth, contentHeight);

  ui.Offset point(double x, double y) => ui.Offset(
        offsetX + x * scale,
        offsetY + y * scale,
      );

  ui.Rect rect(double left, double top, double right, double bottom) {
    return ui.Rect.fromLTRB(
      offsetX + left * scale,
      offsetY + top * scale,
      offsetX + right * scale,
      offsetY + bottom * scale,
    );
  }
}
