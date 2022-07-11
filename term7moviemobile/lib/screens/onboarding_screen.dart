import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/onboarding_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';

class OnBoardingScreen extends StatelessWidget {
  final controller = OnBoardingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: SafeArea(
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            TextButton(
                onPressed: () {
                  controller.storeOnboardInfo();
                  controller.skipAction();
                },
                child: Text(
                  "Skip",
                  style: TextStyle(
                    color: MyTheme.primaryColor,
                  ),
                )),
            Expanded(
              child: PageView.builder(
                controller: controller.pageController,
                itemCount: controller.data.length,
                onPageChanged: controller.currentPage,
                itemBuilder: (context, index) => OnBoardingContent(
                  image: controller.data[index].image,
                  text: controller.data[index].text,
                ),
              ),
            ),
            Row(
              children: [
                ...List.generate(
                  controller.data.length,
                  (index) => Obx(() {
                    return Padding(
                      padding: const EdgeInsets.only(right: 4.0),
                      child:
                          Dot(isActive: index == controller.currentPage.value),
                    );
                  }),
                ),
                Spacer(),
                SizedBox(
                  height: 60,
                  width: 60,
                  child: ElevatedButton(
                    child: Icon(
                      Icons.arrow_forward_rounded,
                      color: Colors.white,
                    ),
                    style: ElevatedButton.styleFrom(
                        shape: CircleBorder(), primary: MyTheme.primaryColor),
                    onPressed: () {
                      controller.storeOnboardInfo();
                      controller.forwardAction();
                    },
                  ),
                ),
              ],
            )
          ],
        ),
      ),
    ));
  }
}

class OnBoardingContent extends StatelessWidget {
  const OnBoardingContent({
    Key? key,
    required this.text,
    required this.image,
  }) : super(key: key);
  final String text, image;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        const Spacer(),
        Image.asset(
          image,
          height: 250,
        ),
        const Spacer(),
        Text(
          text,
          style: Theme.of(context)
              .textTheme
              .headline6
              ?.copyWith(fontWeight: FontWeight.w500),
          textAlign: TextAlign.center,
        ),
        const Spacer(),
      ],
    );
  }
}

class Dot extends StatelessWidget {
  const Dot({
    Key? key,
    this.isActive = false,
  }) : super(key: key);
  final bool isActive;

  @override
  Widget build(BuildContext context) {
    return AnimatedContainer(
      duration: Duration(milliseconds: 200),
      height: isActive ? 12 : 4,
      width: 4,
      decoration: BoxDecoration(
          color: isActive ? MyTheme.primaryColor : MyTheme.grayColor,
          borderRadius: BorderRadius.all(Radius.circular(12))),
    );
  }
}
