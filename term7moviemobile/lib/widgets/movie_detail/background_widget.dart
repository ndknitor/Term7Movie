import 'package:flutter/material.dart';

class BackgroundWidget extends StatelessWidget {
  const BackgroundWidget({
    Key? key,
    required this.size,
    required this.coverImgUrl,
  }) : super(key: key);

  final String coverImgUrl;
  final Size size;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Container(
          height: size.height / 3,
          decoration: BoxDecoration(
              image: DecorationImage(
                  fit: BoxFit.cover, image: NetworkImage(coverImgUrl)),
              ),
        ),
      ],
    );
  }
}
