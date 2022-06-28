
import 'package:flutter/material.dart';
import 'package:term7moviemobile/utils/theme.dart';

class TrailerBar extends StatelessWidget {
  const TrailerBar({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Padding(
            padding: const EdgeInsets.only(
                left: 24),
            child: Stack(
              children: [
                Container(
                  height: 160,
                  width: 260,
                  decoration: BoxDecoration(
                      image: DecorationImage(
                          image: NetworkImage("https://www.youtube.com/watch?v=nBNtRvpCmms&feature=emb_title&ab_channel=CGVCinemasVietnam"),
                          fit: BoxFit.cover),),
                ),
                Container(
                    height: 160,
                    width: 260,
                    decoration:
                    const BoxDecoration(
                        color: Colors
                            .black12)),
                GestureDetector(
                  onTap: () {
                    //play something
                  },
                  child: SizedBox(
                    height: 160,
                    width: 260,
                    child: Container(
                      margin: const EdgeInsets
                          .symmetric(
                          vertical: 56),
                      decoration:
                      const BoxDecoration(
                          shape: BoxShape
                              .circle,
                          color: MyTheme.primaryColor),
                      child: Icon(Icons.play_arrow_rounded,
                            color: Colors.white, size: 16),
                    ),
                  ),
                )
              ],
            ),
          ),
    );
  }
}
