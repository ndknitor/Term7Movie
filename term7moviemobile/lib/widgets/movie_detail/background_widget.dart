import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

class BackgroundWidget extends StatefulWidget {
  const BackgroundWidget({
    Key? key,
    required this.size,
    required this.coverImgUrl,
  }) : super(key: key);

  final String coverImgUrl;
  final Size size;

  @override
  State<BackgroundWidget> createState() => _BackgroundWidgetState();
}

class _BackgroundWidgetState extends State<BackgroundWidget> {
  @override
  Widget build(BuildContext context) {


    return Column(
      children: [
        Container(
          height: widget.size.height / 3,
          decoration: BoxDecoration(
            image: DecorationImage(
              fit: BoxFit.cover,
              image: NetworkImage(widget.coverImgUrl),
            ),
          ),
        ),
      ],
    );
  }
}
