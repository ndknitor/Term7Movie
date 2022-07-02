import 'package:flutter/material.dart';
import 'package:term7moviemobile/utils/theme.dart';
class CasterItem extends StatelessWidget {
  const CasterItem({Key? key, required this.size, required this.cast})
      : super(key: key);

  final Size size;
  final dynamic cast;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Padding(
          padding: const EdgeInsets.only(left: 10, bottom: 4),
          child: Container(
            width: size.width / 4.5,
            height: size.width / 4.5,
            decoration: BoxDecoration(
                image: DecorationImage(image: NetworkImage(cast['profileImageUrl']))),
          ),
        ),
        Text(
          cast['name'],
          style: TextStyle(
              fontWeight: FontWeight.w400,
              fontSize: 16,
              height: 1.2,
              decoration: TextDecoration.none,
              color: MyTheme.grayColor),
        )
      ],
    );
  }
}