import 'package:flutter/material.dart';
import 'caster_item.dart';

class CastBar extends StatelessWidget {
  const CastBar({
    Key? key,
    required this.size,
    required this.list,
  }) : super(key: key);

  final List<dynamic> list;
  final Size size;

  @override
  Widget build(BuildContext context) {

    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        children: list
            .map((e) => Builder(builder: (context) {
                  return CasterItem(
                    size: size,
                    cast: e,
                  );
                }))
            .toList(),
      ),
    );
  }
}
