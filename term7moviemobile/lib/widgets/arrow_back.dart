import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ArrowBack extends StatelessWidget {
  final Color color;
  final callback;
  const ArrowBack({
    Key? key,
    required this.color,
    this.callback,
  }) : super(key: key);


  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 40),
      child: IconButton(
        onPressed: () {
          callback != null ? callback() : Get.back();
        },
        icon: Icon(Icons.keyboard_arrow_left,
            color: color, size: 22),
      ),
    );
  }
}
