import 'package:flutter/material.dart';

class DetailScreen extends StatefulWidget {
  const DetailScreen({Key? key}) : super(key: key);

  @override
  State<DetailScreen> createState() => _DetailScreenState();
}

class _DetailScreenState extends State<DetailScreen> {
  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
        Container(
        // 40% of our total height
        height: size.height * 0.5,
          child: Stack(
            children: <Widget>[
              Container(
                height: size.height * 0.55 - 50,
                decoration: BoxDecoration(
                  //borderRadius: BorderRadius.all(Radius.circular(15)),
                  image: DecorationImage(
                    fit: BoxFit.cover,
                    image: NetworkImage('https://cdnimg.vietnamplus.vn/t1200/Uploaded/Mtpyelagtpy/2019_04_29/avengersendgame2904.jpg'),
                  ),
                ),
              ),
              // Rating Box
              // Positioned(
              //   bottom: 0,
              //   right: 0,
              //   child: Container(
              //     // it will cover 90% of our total width
              //     width: size.width * 0.9,
              //     height: 100,
              //     decoration: BoxDecoration(
              //       color: Colors.white,
              //       borderRadius: BorderRadius.only(
              //         bottomLeft: Radius.circular(50),
              //         topLeft: Radius.circular(50),
              //       ),
              //       boxShadow: [
              //         BoxShadow(
              //           offset: Offset(0, 5),
              //           blurRadius: 50,
              //           color: Color(0xFF12153D).withOpacity(0.2),
              //         ),
              //       ],
              //     ),
              //     child: Padding(
              //       padding: EdgeInsets.symmetric(horizontal: 16),
              //       child: Row(
              //         mainAxisAlignment: MainAxisAlignment.spaceAround,
              //         children: <Widget>[
              //           Column(
              //             mainAxisAlignment: MainAxisAlignment.center,
              //             children: <Widget>[
              //               // SvgPicture.asset("assets/icons/star_fill.svg"),
              //               SizedBox(height: 16 / 4),
              //               // RichText(
              //               //   text: TextSpan(
              //               //     style: TextStyle(color: Colors.black),
              //               //     children: [
              //               //       TextSpan(
              //               //         text: "string",
              //               //         style: TextStyle(
              //               //             fontSize: 16, fontWeight: FontWeight.w600),
              //               //       ),
              //               //       TextSpan(text: "10\n"),
              //               //       TextSpan(
              //               //         text: "150,212",
              //               //         style: TextStyle(color: Colors.white),
              //               //       ),
              //               //     ],
              //               //   ),
              //               // ),
              //             ],
              //           ),
              //           Column(
              //             mainAxisAlignment: MainAxisAlignment.center,
              //             children: <Widget>[
              //               Container(
              //                 padding: EdgeInsets.all(6),
              //                 decoration: BoxDecoration(
              //                   color: Color(0xFF51CF66),
              //                   borderRadius: BorderRadius.circular(2),
              //                 ),
              //                 child: Text(
              //                   "5.9",
              //                   style: TextStyle(
              //                     fontSize: 16,
              //                     color: Colors.white,
              //                     fontWeight: FontWeight.w500,
              //                   ),
              //                 ),
              //               ),
              //               SizedBox(height: 16 / 4),
              //               Text(
              //                 "Meta score",
              //                 style: TextStyle(
              //                     fontSize: 16, fontWeight: FontWeight.w500),
              //               ),
              //               Text(
              //                 "62 critic reviews",
              //                 style: TextStyle(color: Colors.white),
              //               )
              //             ],
              //           )
              //         ],
              //       ),
              //     ),
              //   ),
              // ),
              // Back Button
              // SafeArea(child: BackButton()),
            ],
          ),
        ),
            Padding(
              padding: EdgeInsets.symmetric(
                vertical: 16 / 2,
                horizontal: 16,
              ),
              child: Text(
                'Avengers: End Game',
                style: TextStyle(
                  fontSize: 24.0,
                  fontWeight: FontWeight.bold,
                ),
                textAlign: TextAlign.center,
              ),
            ),
            SizedBox(height: 16 / 2),
            // TitleDurationAndFabBtn(movie: movie),
            // Genres(movie: movie),
            Padding(
              padding: EdgeInsets.symmetric(
                vertical: 16 / 2,
                horizontal: 16,
              ),
              child: Text(
                "Description",
                style: Theme.of(context).textTheme.headline6,
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 16),
              child: Text(
                'Avengers: Hồi kết là phim điện ảnh siêu anh hùng Mỹ ra mắt năm 2019, do Marvel Studios sản xuất và Walt Disney Studios Motion Pictures phân phối độc quyền tạithị trường Bắc Mỹ',
                style: TextStyle(
                  color: Color(0xFF737599),
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(
                vertical: 16 / 2,
                horizontal: 16,
              ),
              child: Text(
                "Actors",
                style: Theme.of(context).textTheme.headline6,
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 16),
              child: Text(
                'Robert Downey Jr. Chris Evans Mark Ruffalo Chris Hemsworth',
                style: TextStyle(
                  color: Color(0xFF737599),
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(
                vertical: 16 / 2,
                horizontal: 16,
              ),
              child: Text(
                "Director",
                style: Theme.of(context).textTheme.headline6,
              ),
            ),
            Padding(
              padding: EdgeInsets.symmetric(horizontal: 16),
              child: Text(
                'Anthony Russo, Joseph V. "Joe" Russo',
                style: TextStyle(
                  color: Color(0xFF737599),
                ),
              ),
            ),
            const SizedBox(
              height: 10,
            ),
            Row(
              children: [
                Padding(
                  padding: EdgeInsets.symmetric(horizontal: 16),
                  child: ElevatedButton(
                    onPressed: () {},
                    child: Text('Book Ticket'),
                  )
                ),
                Padding(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: OutlinedButton(
                      onPressed: () {},
                      child: Text('View Trailer'),
                    )
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
