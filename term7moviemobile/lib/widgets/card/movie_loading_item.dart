import 'package:flutter/material.dart';
import 'package:shimmer/shimmer.dart';

class MovieLoadingItem extends StatelessWidget {
  const MovieLoadingItem({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.only(top: 10, left: 20.0, right: 10),
      decoration: BoxDecoration(
        // color: Colors.white,
        borderRadius: BorderRadius.all(Radius.circular(10)),
        boxShadow: <BoxShadow>[
          BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              offset: const Offset(0, 2),
              blurRadius: 10.0),
        ],
      ),
      child: Shimmer.fromColors(
        highlightColor: Colors.white,
        baseColor: Colors.grey.shade400,
        child: Column(
          mainAxisSize: MainAxisSize.max,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            ClipRRect(
              borderRadius: BorderRadius.circular(10),
              child: Container(
                height: 180,
                width: 150,
                color: Colors.grey,
              ),
            ),
            const SizedBox(
              height: 8,
            ),
            Container(
              width: 150,
              height: 20,
              color: Colors.grey,
            ),
          ],
        ),
      ),
    );
  }
}
