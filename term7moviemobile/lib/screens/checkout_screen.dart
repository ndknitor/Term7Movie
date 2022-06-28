import 'package:flutter/material.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';

class CheckOutScreen extends StatelessWidget {
  const CheckOutScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    return Scaffold(
        body: SingleChildScrollView(
      child: SafeArea(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                ArrowBack(color: MyTheme.bottomBarColor),
                Text("03:00"),
              ],
            ),
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 24),
              padding: const EdgeInsets.symmetric(vertical: 24),
              decoration: const BoxDecoration(
                border: Border(
                    bottom: BorderSide(color: MyTheme.borderColor, width: 1)),
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Container(
                    width: size.width / 3,
                    alignment: Alignment.centerLeft,
                    child: Image.network(
                      Constants.defaultImage,
                      scale: 1.2,
                    ),
                  ),
                  MovieInfoWidget(size: size)
                ],
              ),
            ),
            buildPriceTag('ID Order', '22081996'),
            buildPriceTag('Cinema', 'Beta Quang Trung'),
            buildPriceTag('Date & Time', 'Sun May 22,  16:40'),
            buildPriceTag('Seat Number', 'D7, D8, D9'),
            buildPriceTag('Price', '50.000 VND x 3'),
            buildPriceTag('Total', '150.000 VND'),
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 24),
              padding: const EdgeInsets.only(bottom: 24),
              decoration: const BoxDecoration(
                  border: Border(
                      bottom:
                          BorderSide(color: MyTheme.borderColor, width: 1))),
            ),
            SizedBox(
              height: 24,
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: Container(
                      height: size.height / 16,
                      width: size.width / 2,
                      decoration: BoxDecoration(
                          color: MyTheme.primaryColor,
                          borderRadius: BorderRadius.circular(20)),
                      alignment: Alignment.center,
                      child: const Text(
                        'Check Out',
                        style: TextStyle(
                            fontWeight: FontWeight.w500, color: Colors.white),
                      ),
                    ),
                  ),
                ),
              ],
            )
          ],
        ),
      ),
    ));
  }

  Container buildPriceTag(String content, String price) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 24),
      padding: const EdgeInsets.symmetric(vertical: 12),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(
            content,
          ),
          Text(
            price,
            style: TextStyle(fontWeight: FontWeight.w500),
          ),
        ],
      ),
    );
  }
}

class MovieInfoWidget extends StatelessWidget {
  const MovieInfoWidget({
    Key? key,
    required this.size,
  }) : super(key: key);

  final Size size;

  @override
  Widget build(BuildContext context) {
    return Expanded(
        child: Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Container(
          padding: const EdgeInsets.only(left: 8, bottom: 8),
          width: size.width,
          child: const Text(
            'Ralph Break the Internet',
            style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
          ),
        ),
        Container(
          padding: const EdgeInsets.only(left: 8, bottom: 8),
          width: size.width,
          child: const Text(
            'Action & adventure, Comedy',
          ),
        ),
        Container(
          padding: const EdgeInsets.only(left: 8, bottom: 8),
          width: size.width,
          child: const Text(
            '1h41min',
          ),
        )
      ],
    ));
  }
}
