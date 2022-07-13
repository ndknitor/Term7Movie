import 'package:flutter/material.dart';
import 'package:shimmer/shimmer.dart';

class TransactionLoadingItem extends StatelessWidget {
  const TransactionLoadingItem({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.only(top: 12),
      padding: EdgeInsets.symmetric(vertical: 8, horizontal: 12),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.all(Radius.circular(8)),
        boxShadow: <BoxShadow>[
          BoxShadow(
              color: Colors.grey.withOpacity(0.2),
              offset: const Offset(0, 2),
              blurRadius: 10.0),
        ],
      ),
      child: Shimmer.fromColors(
        highlightColor: Colors.white,
        baseColor: Colors.grey.shade300,
        child: Row(
          mainAxisSize: MainAxisSize.max,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            ClipRRect(
              borderRadius: BorderRadius.circular(10),
              child: Container(
                height: 130,
                width: 90,
                color: Colors.grey,
              ),
            ),
            const SizedBox(
              width: 2,
            ),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Container(
                  width: 150,
                  height: 20,
                  color: Colors.grey,
                ),
                const SizedBox(
                  height: 2,
                ),
                Container(
                  width: 150,
                  height: 20,
                  color: Colors.grey,
                ),
                const SizedBox(
                  height: 4,
                ),
                Container(
                  width: 150,
                  height: 20,
                  color: Colors.grey,
                ),
                const SizedBox(
                  height: 4,
                ),
                Container(
                  width: 150,
                  height: 20,
                  color: Colors.grey,
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
