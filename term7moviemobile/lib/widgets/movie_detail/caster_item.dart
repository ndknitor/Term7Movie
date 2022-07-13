import 'package:flutter/material.dart';
import 'package:term7moviemobile/utils/constants.dart';
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
          padding: const EdgeInsets.only(left: 12, bottom: 4, right: 12),
          child: Container(
            width: size.width / 5,
            height: size.width / 5,
            decoration: BoxDecoration(
                image: DecorationImage(image: AssetImage(Constants.defaultAvatar)),
            borderRadius:  BorderRadius
                .circular(50),),
          ),
        ),
        Padding(padding: const EdgeInsets.only(right: 8), child: Text(
          cast,
          style: TextStyle(
              fontWeight: FontWeight.w400,
              fontSize: 12,
              color: MyTheme.grayColor),
        ),)
      ],
    );
  }
}