import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ArrowBack extends StatelessWidget {
  final Color color;
  const ArrowBack({
    Key? key,
    required this.color,
  }) : super(key: key);


  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 40),
      child: IconButton(
        onPressed: () {
          Get.back();
        },
        icon: Icon(Icons.keyboard_arrow_left,
            color: color, size: 22),
      ),
    );
  }
}
