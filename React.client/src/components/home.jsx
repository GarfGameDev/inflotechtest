
import Markdown from 'react-markdown';
import Readme from './README.md?raw';

const markdown = Readme;
const Home = () => {
    return (
        
        <Markdown>{markdown}</Markdown>
    )
}
export default Home;
