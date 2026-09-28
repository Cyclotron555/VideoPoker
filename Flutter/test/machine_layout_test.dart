import 'package:flutter_test/flutter_test.dart';
import 'package:red_black_poker/game/layout/machine_layout.dart';

void main() {
  test('design-size viewport maps one-to-one', () {
    const layout = MachineLayout(
      MachineLayout.designWidth,
      MachineLayout.designHeight,
    );

    expect(layout.scale, 1);
    expect(layout.offsetX, 0);
    expect(layout.offsetY, 0);

    final rect = layout.rect(100, 200, 300, 500);
    expect(rect.left, 100);
    expect(rect.top, 200);
    expect(rect.right, 300);
    expect(rect.bottom, 500);
  });

  test('tall phone uses one uniform scale and vertical letterboxing', () {
    const layout = MachineLayout(1440, 3088);

    expect(layout.scale, closeTo(1440 / MachineLayout.designWidth, 1e-9));
    expect(layout.contentWidth, closeTo(1440, 1e-6));
    expect(layout.contentHeight, lessThan(3088));
    expect(layout.offsetX, closeTo(0, 1e-6));
    expect(layout.offsetY, greaterThan(0));

    final designRect = layout.rect(
      36,
      930,
      184,
      1182,
    );

    final sx = designRect.width / (184 - 36);
    final sy = designRect.height / (1182 - 930);
    expect(sx, closeTo(sy, 1e-9));
    expect(sx, closeTo(layout.scale, 1e-9));
  });

  test('wide viewport uses one uniform scale and horizontal letterboxing', () {
    const layout = MachineLayout(2000, 1633);

    expect(layout.scale, closeTo(1, 1e-9));
    expect(layout.contentHeight, closeTo(1633, 1e-6));
    expect(layout.offsetX, greaterThan(0));
    expect(layout.offsetY, closeTo(0, 1e-6));
  });
}
