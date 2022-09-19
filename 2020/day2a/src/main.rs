use text_io::scan;

fn main() {
    let input = include_str!("../input.txt");
    
    let mut n_valid_passwords = 0;
    for line in input.lines() {
        let low: usize;
        let high: usize;
        let letter: char;
        let password: String;
        scan!(line.bytes() => "{}-{} {}: {}", low, high, letter, password);

        let letter_count = password.chars().filter(|c| c == &letter).count();
        if low <= letter_count && letter_count <= high {
            n_valid_passwords += 1
        }
    }
    println!("{}", n_valid_passwords)
}
