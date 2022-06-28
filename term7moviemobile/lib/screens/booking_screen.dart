import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';

class BookingScreen extends StatefulWidget {
  const BookingScreen({Key? key}) : super(key: key);

  @override
  State<BookingScreen> createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  final List<String> seatRow = ['A', 'B', 'C', 'D', 'E'];
  final List<String> seatNumber = ['1', '2', '3', '4', '5', '6'];

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    return Scaffold(
      body: SingleChildScrollView(
        child: SafeArea(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const ArrowBack(color: MyTheme.bottomBarColor,),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start  ,
                children: [
                  Container(
                    margin: const EdgeInsets.only(left: 24, top: 8),
                    child: const Text(
                      'Ralph Breaks the Internet',
                      style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
                    ),
                  ),
                  Container(
                    margin: const EdgeInsets.only(top: 4, left: 24),
                    child: const Text(
                      'Beta Quang Trung',
                    ),
                  ),
                ],
              ),
              // seat status bar
              Padding(
                padding: const EdgeInsets.only(top: 24),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    buildSeatStatusBar(
                        color: MyTheme.bottomBarColor, content: 'Available'),
                    buildSeatStatusBar(
                        color: MyTheme.errorColor, content: 'Booked'),
                    buildSeatStatusBar(
                        color: MyTheme.primaryColor, content: 'Your select'),
                  ],
                ),
              ),
              Row(
                children: [
                  Expanded(
                    child: Padding(
                      padding:
                      const EdgeInsets.symmetric(horizontal: 24.0, vertical: 24.0),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: seatRow
                            .map((row) => Builder(builder: (context) {
                          return Container(
                            margin: EdgeInsets.only(top: 8),
                            child: Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: seatNumber.map((number) {
                                  return ToggleButton(
                                    child: Text(
                                      row + number,
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  );
                                }).toList()),
                          );
                        }))
                            .toList(),
                      ),
                    ),
                  ),
                ],
              ),
              Container(
                alignment: Alignment.center,
                child: const Text(
                  'Screen',
                ),
              ),
              Image.asset("assets/images/screen.png"),
              Padding(
                padding: const EdgeInsets.only(left: 24, right: 24, bottom: 16),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: const [
                        Text(
                          'Total Price',
                        ),
                        Text(
                          '150.000 VND',
                          style: TextStyle(fontSize: 18, fontWeight: FontWeight.w600),
                        ),
                      ],
                    ),
                    GestureDetector(
                      onTap: () {
                        Get.toNamed("/checkout");
                      },
                      child: Container(
                        height: size.height / 16,
                        width: size.width / 3,
                        alignment: Alignment.center,
                        decoration: BoxDecoration(
                            color: MyTheme.primaryColor,
                            borderRadius: BorderRadius.circular(16)),
                        child: const Text(
                          'Book Ticket',
                          style: TextStyle(fontWeight: FontWeight.w500, color: Colors.white),
                        ),
                      ),
                    )
                  ],
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  Row buildSeatStatusBar({required Color color, required String content}) {
    return Row(
      children: [
        Container(
          height: 24,
          width: 24,
          decoration: BoxDecoration(
              color: color, borderRadius: BorderRadius.circular(4)),
        ),
        Padding(
          padding: EdgeInsets.only(left: 8.0),
          child: Text(
            content,
            //style: TxtStyle.heading4,
          ),
        ),
      ],
    );
  }
}

class ToggleButton extends StatefulWidget {
  const ToggleButton({Key? key, required this.child}) : super(key: key);
  final Widget child;

  @override
  State<ToggleButton> createState() => _ToggleButtonState();
}

class _ToggleButtonState extends State<ToggleButton> {
  var isUsed = false;
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 40,
      width: 40,
      child: Expanded(
        child: GestureDetector(
          onTap: () {
            setState(() {
              isUsed = !isUsed;
            });
          },
          child: Container(
            alignment: Alignment.center,
            decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(8),
                color: !isUsed
                    ? MyTheme.grayColor
                    : MyTheme.primaryColor),
            child: widget.child,
          ),
        ),
      ),
    );
  }
}
